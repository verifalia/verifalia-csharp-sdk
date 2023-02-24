/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2021 Cobisi Research
*
* Cobisi Research
* Via Della Costituzione, 31
* 35010 Vigonza
* Italy - European Union
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Verifalia.Api.Exceptions;
using Verifalia.Api.Security;

namespace Verifalia.Api
{
    internal sealed class MultiplexedRestClient : IRestClient
    {
        private readonly IAuthenticationProvider _authenticator;
        private int _currentBaseUrlIdx = 0;
        private readonly Url[] _baseUrls;

        public IFlurlClient UnderlyingClient { get; }

        public MultiplexedRestClient(IAuthenticationProvider authenticator, string userAgent, IEnumerable<Uri> baseUris)
        {
            if (userAgent == null) throw new ArgumentNullException(nameof(userAgent));
            if (baseUris == null) throw new ArgumentNullException(nameof(baseUris));
            
            _authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));
            _baseUrls = baseUris
                .Select(uri => new Url(uri.AbsoluteUri))
                .ToArray();
            
            // Setup the underlying Flurl instance

            UnderlyingClient = new FlurlClient()
                .Configure(settings =>
                {
                    settings.JsonSerializer = new ProgressiveJsonSerializer();

                    // HTTP status codes manually handled by this multiplexer

                    settings.AllowedHttpStatusRange = "*";
                })
                .WithHeader("Accept-Encoding", "gzip, deflate")
                .AllowAnyHttpStatus();

            UnderlyingClient
                .HttpClient
                .DefaultRequestHeaders
                .TryAddWithoutValidation("User-Agent", userAgent);
        }

        public async Task<HttpResponseMessage> InvokeAsync(HttpMethod verb, string resource, Dictionary<string, string>? queryParams = null,
            Dictionary<string, object>? headers = null, Func<CancellationToken, Task<HttpContent>>? contentFactory = null,
            bool bufferResponseContent = true, bool skipAuthentication = false, CancellationToken cancellationToken = default)
        {
            if (verb == null) throw new ArgumentNullException(nameof(verb));
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            
            // Performs a maximum of as many attempts as the number of configured base API endpoints, keeping track
            // of the last used endpoint after each call, in order to try to distribute the load evenly across the
            // available endpoints.

            var errors = new Dictionary<Url, Exception>();

            for (var idxAttempt = 0; idxAttempt < _baseUrls.Length; idxAttempt++, _currentBaseUrlIdx++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Authenticates the underlying flurl client, if needed

                if (!skipAuthentication)
                {
                    await _authenticator.AuthenticateAsync(this, cancellationToken)
                        .ConfigureAwait(false);
                }

                // Build the final url by combining the base url and the specified path and query

                var baseUrl = _baseUrls[_currentBaseUrlIdx % _baseUrls.Length];
                var finalUrl = new Url(baseUrl)
                    .AppendPathSegment(resource);

                // Add the eventual querystring parameters

                if (queryParams != null)
                {
                    foreach (var queryParam in queryParams)
                    {
                        finalUrl = finalUrl.SetQueryParam(queryParam.Key, queryParam.Value);
                    }
                }

                var request = UnderlyingClient.Request(finalUrl);

                // Add the eventual custom headers

                if (headers != null)
                {
                    foreach (var queryParam in headers)
                    {
                        request = request.WithHeader(queryParam.Key, queryParam.Value);
                    }
                }

                try
                {
                    // HttpClient.SendAsync() disposed the passed content automatically: https://github.com/dotnet/runtime/issues/14612
                    // A fix was made (https://github.com/dotnet/corefx/pull/19082) but Flurl still targets .NET Standard and not .NET Core,
                    // so we can't take advantage of it: because of that, we are using a factory to build the actual content.

                    HttpContent? content = null;
                    HttpResponseMessage response;

                    try
                    {
                        if (contentFactory != null)
                        {
                            content = await contentFactory(cancellationToken)
                                .ConfigureAwait(false);
                        }

                        response =
                            (
                                await request
                                    .SendAsync(verb,
                                        content,
                                        cancellationToken,
                                        bufferResponseContent
                                            ? HttpCompletionOption.ResponseContentRead
                                            : HttpCompletionOption.ResponseHeadersRead)
                                    .ConfigureAwait(false)
                            )
#if USE_FLURL_3
                            // Flurl v3.x compatibility
                            .ResponseMessage
#endif
                            ;
                    }
                    finally
                    {
                        // Dispose the eventual content - that should make sense whenever .NET Standard will receive the fix mentioned above
                        // or whenever Flurl will target .NET Core.

                        content?.Dispose();
                    }

                    // Automatically retry with another host on HTTP 5xx status codes

                    if ((int) response.StatusCode >= 500 && (int) response.StatusCode <= 599)
                    {
                        errors.Add(finalUrl, new EndpointServerErrorException($"The API endpoint {baseUrl} returned a server error HTTP status code {response.StatusCode}."));
                        continue;
                    }

                    // If the request is unauthorized, give the authentication provider a chance to remediate (on a subsequent attempt)

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        await _authenticator.HandleUnauthorizedRequestAsync(this, cancellationToken)
                            .ConfigureAwait(false);

                        errors.Add(finalUrl, new AuthorizationException("Can't authenticate to Verifalia using the provided credentials (will retry in the next attempt)."));
                        continue;
                    }

                    // Fails on the first occurrence of an HTTP 403 status code

                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        throw new AuthorizationException(response.ReasonPhrase);
                    }

                    // Returns the original response only if it has been completed with a non-500 HTTP status code

                    return response;
                }
                catch (AggregateException aggregateException)
                {
                    var flattenException = aggregateException.Flatten();

                    foreach (var innerException in flattenException.InnerExceptions)
                    {
                        if (innerException is OperationCanceledException)
                        {
                            throw innerException;
                        }

                        errors.Add(finalUrl, innerException);
                    }
                }
                catch (FlurlHttpTimeoutException httpException)
                {
                    // A single Flurl request timed out - we take note of it and proceed with the next endpoint, if any
                    
                    errors.Add(finalUrl, httpException);
                }
                catch (FlurlHttpException httpException)
                {
                    if (httpException.InnerException is OperationCanceledException && cancellationToken.IsCancellationRequested)
                    {
                        throw httpException.InnerException;
                    }

                    errors.Add(finalUrl, httpException);
                }
            }

            throw new ServiceUnreachableException("All the base URIs are unreachable: " + String.Join(", ", errors.Select(error => $"{error.Key} => {error.Value.Message}")),
                new AggregateException(errors.Select(error => error.Value)));
        }

        public T Deserialize<T>(Stream stream)
        {
            return UnderlyingClient.Settings.JsonSerializer.Deserialize<T>(stream);
        }

        public string Serialize(object obj)
        {
            return UnderlyingClient.Settings.JsonSerializer.Serialize(obj);
        }

        public void Dispose()
        {
            UnderlyingClient?.Dispose();
        }
    }
}
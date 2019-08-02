/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2019 Cobisi Research
*
* Cobisi Research
* Via Prima Strada, 35
* 35129, Padova
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
        private readonly IFlurlClient _underlyingClient;
        private readonly Url[] _baseUrls;

        public MultiplexedRestClient(IAuthenticator authenticator, string userAgent, IEnumerable<Uri> baseUris)
            : this(baseUris)
        {
            if (authenticator == null) throw new ArgumentNullException(nameof(authenticator));
            if (userAgent == null) throw new ArgumentNullException(nameof(userAgent));

            // Setup the underlying Flurl instance

            _underlyingClient = new FlurlClient()
                .Configure(settings =>
                {
                    settings.JsonSerializer = new ProgressiveJsonSerializer();

                    // HTTP status codes manually handled by this multiplexer

                    settings.AllowedHttpStatusRange = "*";
                })
                .AddAuthentication(authenticator)
                .WithHeader("Accept-Encoding", "gzip, deflate")
                .AllowAnyHttpStatus();

            _underlyingClient
                .HttpClient
                .DefaultRequestHeaders
                .TryAddWithoutValidation("User-Agent", userAgent);
        }

        public MultiplexedRestClient(IFlurlClient underlyingClient, IEnumerable<Uri> baseUris)
            : this(baseUris)
        {
            _underlyingClient = underlyingClient ?? throw new ArgumentNullException(nameof(underlyingClient));
        }

        private MultiplexedRestClient(IEnumerable<Uri> baseUris)
        {
            if (baseUris == null) throw new ArgumentNullException(nameof(baseUris));

            _baseUrls = baseUris
                .Select(uri => new Url(uri.AbsoluteUri))
                .ToArray();
        }


        public async Task<HttpResponseMessage> InvokeAsync(HttpMethod verb, string resource, Dictionary<string, string> queryParams = null, Dictionary<string, object> headers = null, HttpContent content = null, bool bufferResponseContent = true, CancellationToken cancellationToken = default)
        {
            if (verb == null) throw new ArgumentNullException(nameof(verb));
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            var errors = new Dictionary<Url, Exception>();

            foreach (var baseUrl in _baseUrls)
            {
                // Build the final url based on the base url and the specified path and query

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

                var request = _underlyingClient.Request(finalUrl);

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
                    var response = await request
                        .SendAsync(verb,
                            content,
                            cancellationToken,
                            bufferResponseContent
                                ? HttpCompletionOption.ResponseContentRead
                                : HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false);

                    // Automatically retry with another host on HTTP 5xx status codes

                    if ((int)response.StatusCode >= 500 && (int)response.StatusCode <= 599)
                    {
                        errors.Add(finalUrl, new EndpointServerErrorException($"The API endpoint {baseUrl} returned a server error HTTP status code {response.StatusCode}."));

                        continue;
                    }

                    // Fails on the first occurrence of an HTTP 403 status code

                    if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
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
                catch (FlurlHttpException httpException)
                {
                    errors.Add(finalUrl, httpException);
                }
            }

            throw new ServiceUnreachableException("All the base URIs are unreachable: " + String.Join(", ", errors.Select(error => $"{error.Key} => {error.Value.Message}")),
                new AggregateException(errors.Select(error => error.Value)));
        }

        public T Deserialize<T>(Stream stream)
        {
            return _underlyingClient.Settings.JsonSerializer.Deserialize<T>(stream);
        }

        public T Deserialize<T>(string value)
        {
            return _underlyingClient.Settings.JsonSerializer.Deserialize<T>(value);
        }

        public string Serialize(object obj)
        {
            return _underlyingClient.Settings.JsonSerializer.Serialize(obj);
        }

        public void Dispose()
        {
            _underlyingClient?.Dispose();
        }
    }
}
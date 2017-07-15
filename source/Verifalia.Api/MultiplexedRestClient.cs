using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Exceptions;
using Flurl.Http;
using System.Net.Http;
using Flurl;
using Flurl.Http.Configuration;
using Flurl.Http.Content;

namespace Verifalia.Api
{
    internal class MultiplexedRestClient : IRestClient
    {
        private IFlurlClient _underlyingClient;
        private readonly Url[] _baseUrls;

        public MultiplexedRestClient(IEnumerable<Uri> baseUris, string accountSid, string authToken, string userAgent)
        {
            if (baseUris == null) throw new ArgumentNullException(nameof(baseUris));
            if (accountSid == null) throw new ArgumentNullException(nameof(accountSid));
            if (authToken == null) throw new ArgumentNullException(nameof(authToken));
            if (userAgent == null) throw new ArgumentNullException(nameof(userAgent));

            _baseUrls = baseUris
                .Select(uri => new Url(uri.AbsoluteUri))
                .ToArray();

            _underlyingClient = new FlurlClient(new FlurlHttpSettings
                {
                    JsonSerializer = new ProgressiveJsonSerializer(),

                    // HTTP status codes manually handled by this multiplexer

                    AllowedHttpStatusRange = "*"
                })
                .WithUrl(new Uri("http://dummy"))
                .WithBasicAuth(accountSid, authToken)
                .AllowAnyHttpStatus();

            _underlyingClient
                .HttpClient
                .DefaultRequestHeaders
                .TryAddWithoutValidation("User-Agent", userAgent);
        }

        public async Task<HttpResponseMessage> InvokeAsync(HttpMethod verb, string resource, HttpContent content = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (verb == null) throw new ArgumentNullException(nameof(verb));
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            var errors = new Dictionary<Url, string>();

            foreach (var baseUrl in _baseUrls)
            {
                // Build the final url based on the base url and the specified path and query

                var finalUrl = new Url(baseUrl)
                    .AppendPathSegment(resource);

                this._underlyingClient = this._underlyingClient
                    .WithUrl(finalUrl);

                try
                {
                    var response = await _underlyingClient
                        .SendAsync(verb, content, cancellationToken)
                        .ConfigureAwait(false);

                    // Automatically retry with another host on HTTP 500 status codes

                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var responseContent = await response
                            .Content
                            .ReadAsStringAsync()
                            .ConfigureAwait(false);

                        errors.Add(finalUrl, String.Format("HTTP status {0}, message: {1}", response.StatusCode, responseContent));

                        continue;
                    }

                    // Returns the original response only if it has been completed with a non-500 HTTP status code

                    return response;
                }
                catch (OperationCanceledException)
                {
                    throw;
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

                        errors.Add(finalUrl, innerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    errors.Add(finalUrl, exception.Message);
                }
            }

            throw new VerifaliaException("All the base URIs are unreachable: " + String.Join(", ", errors.Select(error => String.Format("{0} => {1}", error.Key, error.Value))));
        }

        public async Task<T> DeserializeContentAsync<T>(HttpResponseMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            // message.Headers[...]

            using (var stream = await message.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                return this._underlyingClient.Settings.JsonSerializer.Deserialize<T>(stream);
            }
        }

        public HttpContent SerializeContent(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return new CapturedJsonContent(this._underlyingClient.Settings.JsonSerializer.Serialize(obj));
        }
    }
}
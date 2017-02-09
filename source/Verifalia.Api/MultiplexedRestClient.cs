using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Verifalia.Api.Exceptions;

namespace Verifalia.Api
{
    internal class MultiplexedRestClient : RestClient
    {
        private readonly IEnumerable<Uri> _baseUris;

        public MultiplexedRestClient(IEnumerable<Uri> baseUris)
        {
            if (baseUris == null) throw new ArgumentNullException("baseUris");

            _baseUris = baseUris;
        }

        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {
            var errors = new Dictionary<Uri, string>();

            foreach (var baseUri in _baseUris)
            {
                this.BaseUrl = baseUri;

                try
                {
                    var response = base.Execute<T>(request);

                    // Returns the original response only if it has been completed successfully

                    if (response.ResponseStatus == ResponseStatus.Completed)
                    {
                        // Automatically retry with another host on HTTP 500 status codes

                        if (response.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            errors.Add(baseUri, response.StatusDescription);
                            continue;
                        }

                        return response;
                    }

                    // The request has NOT been completed

                    errors.Add(baseUri,
                        response.ErrorException == null
                            ? String.Format("Response status: {0}", response.ResponseStatus)
                            : response.ErrorException.Message);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    errors.Add(baseUri, exception.Message);
                }
            }

            throw new VerifaliaException("All the base URIs are unreachable: " + String.Join(", ", errors.Select(error => String.Format("{0} => {1}", error.Key, error.Value))));
        }

        public override async Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            var errors = new Dictionary<Uri, string>();

            foreach (var baseUri in _baseUris)
            {
                this.BaseUrl = baseUri;

                try
                {
                    var response = await base.ExecuteTaskAsync<T>(request, token);

                    // Returns the original response only if it has been completed successfully

                    if (response.ResponseStatus == ResponseStatus.Completed)
                    {
                        // Automatically retry with another host on HTTP 500 status codes

                        if (response.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            errors.Add(baseUri, response.StatusDescription);
                            continue;
                        }

                        return response;
                    }

                    // The request has NOT been completed

                    errors.Add(baseUri,
                        response.ErrorException == null
                            ? String.Format("Response status: {0}", response.ResponseStatus)
                            : response.ErrorException.Message);
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

                        errors.Add(baseUri, innerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    errors.Add(baseUri, exception.Message);
                }
            }

            throw new VerifaliaException("All the base URIs are unreachable: " + String.Join(", ", errors.Select(error => String.Format("{0} => {1}", error.Key, error.Value))));
        }
    }
}
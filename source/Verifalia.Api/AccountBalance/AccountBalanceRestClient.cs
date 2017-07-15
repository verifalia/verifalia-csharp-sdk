﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Exceptions;

namespace Verifalia.Api.AccountBalance
{
    /// <summary>
    /// Allows to manage the credits for the Verifalia account.
    /// <remarks>The functionalities of this type are exposed by way of the <see cref="VerifaliaRestClient.AccountBalance">AccountBalance property</see>
    /// of <see cref="VerifaliaRestClient">VerifaliaRestClient</see>.</remarks>
    /// </summary>
    internal class AccountBalanceRestClient : IAccountBalanceRestClient
    {
        private readonly IRestClientFactory _restClientFactory;

        internal AccountBalanceRestClient(IRestClientFactory restClientFactory)
        {
            if (restClientFactory == null) throw new ArgumentNullException(nameof(restClientFactory));

            _restClientFactory = restClientFactory;
        }

        /// <summary>
        /// Returns the balance for the calling Verifalia account.
        /// </summary>
        public Models.AccountBalance Query()
        {
            try
            {
                return QueryAsync().Result;
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

                    if (innerException is VerifaliaException)
                    {
                        throw innerException;
                    }
                }

                throw new VerifaliaException("One or more errors occurred during the operation; please see the inner exception for further details.", aggregateException);
            }
        }

        /// <summary>
        /// Returns the balance for the calling Verifalia account.
        /// </summary>
        public async Task<Models.AccountBalance> QueryAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Sends the request to the Verifalia servers

            var restClient = _restClientFactory.Build();
            var response = await restClient
                .InvokeAsync(HttpMethod.Get, "account-balance", cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        return await restClient.DeserializeContentAsync<Models.AccountBalance>(response);
                    }
            }

            // An unexpected HTTP status code has been received at this point

            var responseBody = await response
                .Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            throw new VerifaliaException(String.Format("Unexpected HTTP response: {0} {1}", (int)response.StatusCode, responseBody))
            {
                Response = response
            };
        }
    }
}
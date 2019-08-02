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
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Credits.Models;
using Verifalia.Api.Exceptions;
using Verifalia.Api.Common;
using Verifalia.Api.Common.Models;

namespace Verifalia.Api.Credits
{
    /// <inheritdoc />
    internal class CreditsRestClient : ICreditsRestClient
    {
        private readonly IRestClientFactory _restClientFactory;

        internal CreditsRestClient(IRestClientFactory restClientFactory)
        {
            _restClientFactory = restClientFactory ?? throw new ArgumentNullException(nameof(restClientFactory));
        }

        public async Task<Balance> GetBalanceAsync(CancellationToken cancellationToken = default)
        {
            // Sends the request to the Verifalia API

            var restClient = _restClientFactory.Build();

            using (var response = await restClient
                .InvokeAsync(HttpMethod.Get, "credits/balance", cancellationToken: cancellationToken)
                .ConfigureAwait(false))
            {

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response
                        .Content
                        .DeserializeAsync<Balance>(restClient)
                        .ConfigureAwait(false);
                }

                // An unexpected HTTP status code has been received at this point

                var responseBody = await response
                    .Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

                throw new VerifaliaException($"Unexpected HTTP response: {(int)response.StatusCode} {responseBody}");
            }
        }

#if HAS_ASYNC_ENUMERABLE_SUPPORT

        public IAsyncEnumerable<DailyUsage> ListDailyUsagesAsync(DailyUsageListingOptions options = null, CancellationToken cancellationToken = default)
        {
            return AsyncEnumerableHelper
                .ToAsyncEnumerable<DailyUsageListSegment, DailyUsage, DailyUsageListingOptions>(
                    ListDailyUsagesSegmentedAsync,
                    ListDailyUsagesSegmentedAsync,
                    options,
                    cancellationToken);
        }

#endif

        public async Task<DailyUsageListSegment> ListDailyUsagesSegmentedAsync(DailyUsageListingOptions options = null, CancellationToken cancellationToken = default)
        {
            // Generate the additional parameters, where needed

            var restClient = _restClientFactory.Build();

            // Send the request to the Verifalia servers

            Dictionary<string, string> queryParams = null;

            if (options != null)
            {
                queryParams = new Dictionary<string, string>();

                // Standard parameters

                if (options.Limit > 0)
                {
                    queryParams["limit"] = options.Limit.ToString(CultureInfo.InvariantCulture);
                }

                // Predicates

                if (options.DateFilter != null)
                {
                    foreach (var fragment in options.DateFilter.Serialize("date"))
                    {
                        queryParams[fragment.Key] = fragment.Value;
                    }
                }
            }

            return await ListDailyUsageSegmentedImplAsync(restClient, queryParams, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<DailyUsageListSegment> ListDailyUsagesSegmentedAsync(ListingCursor cursor, CancellationToken cancellationToken = default)
        {
            if (cursor == null) throw new ArgumentNullException(nameof(cursor));

            // Generate the additional parameters, where needed

            var restClient = _restClientFactory.Build();

            // Send the request to the Verifalia servers

            var cursorParamName = cursor.Direction == Direction.Forward
                ? "cursor"
                : "cursor:prev";

            var queryParams = new Dictionary<string, string>
            {
                [cursorParamName] = cursor.Cursor
            };

            if (cursor.Limit > 0)
            {
                queryParams["limit"] = cursor.Limit.ToString(CultureInfo.InvariantCulture);
            }

            return await ListDailyUsageSegmentedImplAsync(restClient, queryParams, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<DailyUsageListSegment> ListDailyUsageSegmentedImplAsync(IRestClient restClient, Dictionary<string, string> queryParams, CancellationToken cancellationToken)
        {
            using (var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    "credits/daily-usage",
                    queryParams,
                    headers: new Dictionary<string, object> { { "Accept", WellKnowMimeContentTypes.ApplicationJson } },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false))

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            return await response
                                .Content
                                .DeserializeAsync<DailyUsageListSegment>(restClient)
                                .ConfigureAwait(false);
                        }

                    default:
                        {
                            var responseBody = await response
                                .Content
                                .ReadAsStringAsync()
                                .ConfigureAwait(false);

                            // An unexpected HTTP status code has been received at this point

                            throw new VerifaliaException($"Unexpected HTTP response: {(int)response.StatusCode} {responseBody}");
                        }
                }
        }
    }
}
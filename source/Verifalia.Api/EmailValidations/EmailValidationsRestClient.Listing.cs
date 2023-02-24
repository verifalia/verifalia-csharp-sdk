/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2023 Cobisi Research
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
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Exceptions;
using Verifalia.Api.Common;
using Verifalia.Api.Common.Models;
using Verifalia.Api.EmailValidations.Models;

namespace Verifalia.Api.EmailValidations
{
    /// <inheritdoc />
    internal partial class EmailValidationsRestClient
    {

#if HAS_ASYNC_ENUMERABLE_SUPPORT

        public IAsyncEnumerable<ValidationOverview> ListAsync(ValidationOverviewListingOptions? options = null, CancellationToken cancellationToken = default)
        {
            return AsyncEnumerableHelper
                .ToAsyncEnumerableAsync<ValidationOverviewListSegment, ValidationOverview, ValidationOverviewListingOptions>(
                    ListSegmentedAsync,
                    ListSegmentedAsync,
                    options,
                    cancellationToken);
        }

#endif

        public async Task<ValidationOverviewListSegment> ListSegmentedAsync(ValidationOverviewListingOptions? options = default, CancellationToken cancellationToken = default)
        {
            // Generate the additional parameters, where needed

            var restClient = _restClientFactory.Build();

            // Send the request to the Verifalia servers

            Dictionary<string, string>? queryParams = null;

            if (options != null)
            {
                queryParams = new Dictionary<string, string>();

                if (options.Limit > 0)
                {
                    queryParams["limit"] = options.Limit.ToString(CultureInfo.InvariantCulture);
                }

                switch (options.OrderBy)
                {
                    case ValidationOverviewListingField.CreatedOn:
                        queryParams["sort"] = $"{(options.Direction == Direction.Backward ? "-" : null)}createdOn";
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(options));
                }
            }

            using var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    "email-validations",
                    queryParams: queryParams,
                    headers: new Dictionary<string, object> {{"Accept", WellKnownMimeContentTypes.ApplicationJson}},
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            
            return await ListSegmentedImplAsync(restClient, response, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ValidationOverviewListSegment> ListSegmentedAsync(ListingCursor cursor, CancellationToken cancellationToken = default)
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

            using var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    "email-validations",
                    queryParams,
                    headers: new Dictionary<string, object> {{"Accept", WellKnownMimeContentTypes.ApplicationJson}},
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            
            return await ListSegmentedImplAsync(restClient, response, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<ValidationOverviewListSegment> ListSegmentedImplAsync(IRestClient restClient, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        return await response
                            .Content
                            .DeserializeAsync<ValidationOverviewListSegment>(restClient)
                            .ConfigureAwait(false);
                    }

                default:
                    {
                        var responseBody = await response
                            .Content
#if NET5_0_OR_GREATER
                            .ReadAsStringAsync(cancellationToken)
#else
                            .ReadAsStringAsync()
#endif
                            .ConfigureAwait(false);

                        // An unexpected HTTP status code has been received at this point

                        throw new VerifaliaException($"Unexpected HTTP response: {(int) response.StatusCode} {responseBody}");
                    }
            }
        }
    }
}
/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2020 Cobisi Research
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

        public IAsyncEnumerable<QualityLevel> ListQualityLevelsAsync(ListingOptions options = null, CancellationToken cancellationToken = default)
        {
            return AsyncEnumerableHelper
                .ToAsyncEnumerable<QualityLevelListSegment, QualityLevel, ListingOptions>(
                    ListQualityLevelsSegmentedAsync,
                    ListQualityLevelsSegmentedAsync,
                    options,
                    cancellationToken);
        }

#endif

        public async Task<QualityLevelListSegment> ListQualityLevelsSegmentedAsync(ListingOptions options = default, CancellationToken cancellationToken = default)
        {
            // Generate the additional parameters, where needed

            var restClient = _restClientFactory.Build();

            // Send the request to the Verifalia servers

            Dictionary<string, string> queryParams = null;

            if (options != null)
            {
                queryParams = new Dictionary<string, string>();

                if (options.Limit > 0)
                {
                    queryParams["limit"] = options.Limit.ToString(CultureInfo.InvariantCulture);
                }
            }

            using (var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    "email-validations/quality-levels",
                    queryParams: queryParams,
                    headers: new Dictionary<string, object> {{"Accept", WellKnownMimeContentTypes.ApplicationJson}},
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false))
            {
                return await ListQualityLevelsSegmentedImplAsync(restClient, response)
                    .ConfigureAwait(false);
            }
        }

        public async Task<QualityLevelListSegment> ListQualityLevelsSegmentedAsync(ListingCursor cursor, CancellationToken cancellationToken = default)
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

            using (var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    "email-validations/quality-levels",
                    queryParams,
                    headers: new Dictionary<string, object> {{"Accept", WellKnownMimeContentTypes.ApplicationJson}},
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false))
            {
                return await ListQualityLevelsSegmentedImplAsync(restClient, response)
                    .ConfigureAwait(false);
            }
        }

        private async Task<QualityLevelListSegment> ListQualityLevelsSegmentedImplAsync(IRestClient restClient, HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        return await response
                            .Content
                            .DeserializeAsync<QualityLevelListSegment>(restClient)
                            .ConfigureAwait(false);
                    }

                default:
                    {
                        var responseBody = await response
                            .Content
                            .ReadAsStringAsync()
                            .ConfigureAwait(false);

                        // An unexpected HTTP status code has been received at this point

                        throw new VerifaliaException($"Unexpected HTTP response: {(int) response.StatusCode} {responseBody}");
                    }
            }
        }
    }
}
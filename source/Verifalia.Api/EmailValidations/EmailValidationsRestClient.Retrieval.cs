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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Common;
using Verifalia.Api.Common.Models;
using Verifalia.Api.EmailValidations.Models;
using Verifalia.Api.Exceptions;

namespace Verifalia.Api.EmailValidations
{
    /// <inheritdoc />
    internal partial class EmailValidationsRestClient
    {
        public async Task<Validation> GetAsync(Guid id, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            // Sends the request to the Verifalia servers

            var restClient = _restClientFactory.Build();

            using (var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    $"email-validations/{id:D}",
                    headers: new Dictionary<string, object> {{"Accept", WellKnownMimeContentTypes.ApplicationJson}},
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false))
            {

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    {
                        var partialValidation = await response
                            .Content
                            .DeserializeAsync<PartialValidation>(restClient)
                            .ConfigureAwait(false);

                        // Returns immediately if the validation has been completed or if we should not wait for it

                        if (waitingStrategy == null || !waitingStrategy.WaitForCompletion || partialValidation.Overview.Status == ValidationStatus.Completed)
                        {
                            return await RetrieveValidationFromPartialValidationAsync(partialValidation, cancellationToken)
                                .ConfigureAwait(false);
                        }

                        return await WaitForCompletionAsync<Validation>(partialValidation.Overview, waitingStrategy,
                                cancellationToken)
                            .ConfigureAwait(false);
                    }

                    case HttpStatusCode.Gone:
                    case HttpStatusCode.NotFound:
                    {
                        return null;
                    }

                    default:
                    {
                        // An unexpected HTTP status code has been received at this point

                        var responseBody = await response
                            .Content
#if NET5_0
                            .ReadAsStringAsync(cancellationToken)
#else
                            .ReadAsStringAsync()
#endif
                            .ConfigureAwait(false);

                        throw new VerifaliaException(
                            $"Unexpected HTTP response: {(int) response.StatusCode} {responseBody}");
                    }
                }
            }
        }

        private async Task<Validation> RetrieveValidationFromPartialValidationAsync(PartialValidation partialValidation, CancellationToken cancellationToken)
        {
            if (partialValidation == null) throw new ArgumentNullException(nameof(partialValidation));

            var allEntries = new List<ValidationEntry>(partialValidation.Overview.NoOfEntries);
            var currentSegment = partialValidation.Entries;

            while (currentSegment?.Data != null)
            {
                allEntries.AddRange(currentSegment.Data);

                if (!currentSegment.Meta.IsTruncated)
                {
                    break;
                }

                var listingCursor = new ListingCursor(currentSegment.Meta.Cursor);

                currentSegment = await ListEntriesSegmentedAsync(partialValidation.Overview.Id,
                        listingCursor,
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            return new Validation
            {
                Overview = partialValidation.Overview,
                Entries = allEntries
            };
        }

        public async Task<ValidationOverview> GetOverviewAsync(Guid id, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            // Sends the request to the Verifalia servers

            var restClient = _restClientFactory.Build();

            using (var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    $"email-validations/{id:D}/overview",
                    headers: new Dictionary<string, object> {{"Accept", WellKnownMimeContentTypes.ApplicationJson}},
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false))
            {

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    {
                        var validationOverview = await response
                            .Content
                            .DeserializeAsync<ValidationOverview>(restClient)
                            .ConfigureAwait(false);

                        validationOverview.Status = response.StatusCode == HttpStatusCode.Accepted
                            ? ValidationStatus.InProgress
                            : ValidationStatus.Completed;

                        // Returns immediately if the validation has been completed or if we should not wait for it

                        if (waitingStrategy == null || !waitingStrategy.WaitForCompletion || validationOverview.Status == ValidationStatus.Completed)
                        {
                            return validationOverview;
                        }

                        return await WaitForCompletionAsync<ValidationOverview>(validationOverview, waitingStrategy,
                                cancellationToken)
                            .ConfigureAwait(false);
                    }

                    case HttpStatusCode.Gone:
                    case HttpStatusCode.NotFound:
                    {
                        return null;
                    }
                }

                // An unexpected HTTP status code has been received at this point

                var responseBody = await response
                    .Content
#if NET5_0
                    .ReadAsStringAsync(cancellationToken)
#else
                    .ReadAsStringAsync()
#endif
                    .ConfigureAwait(false);

                throw new VerifaliaException($"Unexpected HTTP response: {(int) response.StatusCode} {responseBody}");
            }
        }


#if HAS_ASYNC_ENUMERABLE_SUPPORT

        public IAsyncEnumerable<ValidationEntry> ListEntriesAsync(Guid validationId, ValidationEntryListingOptions options = default, CancellationToken cancellationToken = default)
        {
            return AsyncEnumerableHelper
                .ToAsyncEnumerable<ValidationEntryListSegment, ValidationEntry, ValidationEntryListingOptions>(
                    (listingOptions, token) => ListEntriesSegmentedAsync(validationId, listingOptions, token),
                    (cursor, token) => ListEntriesSegmentedAsync(validationId, cursor, token),
                    options,
                    cancellationToken);
        }

#endif

        public async Task<ValidationEntryListSegment> ListEntriesSegmentedAsync(Guid validationId, ValidationEntryListingOptions options = default, CancellationToken cancellationToken = default)
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

                // Predicates

                if (options.StatusFilter != null)
                {
                    foreach (var fragment in options.StatusFilter.Serialize("status"))
                    {
                        queryParams[fragment.Key] = fragment.Value;
                    }
                }
            }

            using (var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    $"email-validations/{validationId:D}/entries",
                    queryParams: queryParams,
                    headers: new Dictionary<string, object> { { "Accept", WellKnownMimeContentTypes.ApplicationJson } },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false))
            {
                return await ListEntriesSegmentedImplAsync(restClient, response)
                    .ConfigureAwait(false);
            }
        }

        public async Task<ValidationEntryListSegment> ListEntriesSegmentedAsync(Guid validationId, ListingCursor cursor, CancellationToken cancellationToken = default)
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
                    $"email-validations/{validationId:D}/entries",
                    queryParams,
                    headers: new Dictionary<string, object> { { "Accept", WellKnownMimeContentTypes.ApplicationJson } },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false))
            {
                return await ListEntriesSegmentedImplAsync(restClient, response)
                    .ConfigureAwait(false);
            }
        }

        private async Task<ValidationEntryListSegment> ListEntriesSegmentedImplAsync(IRestClient restClient, HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        return await response
                            .Content
                            .DeserializeAsync<ValidationEntryListSegment>(restClient)
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
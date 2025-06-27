/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2025 Cobisi Research
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
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Common;
using Verifalia.Api.Common.Models;
using Verifalia.Api.EmailVerifications.Models;

namespace Verifalia.Api.EmailVerifications
{
    /// <inheritdoc />
    internal partial class EmailVerificationsClient
    {
        public async Task<Verification?> GetAsync(string verificationId, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default)
        {
            var waitOptionsOrDefault = waitOptions ?? WaitOptions.Default;
            
            // Send the request to the Verifalia servers

            var restClient = _restClientFactory.Build();

            using var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    $"email-validations/{verificationId}",
                    queryParams: new Dictionary<string, string>
                    {
                        {
                            "waitTime", ((int) waitOptionsOrDefault.PollWaitTime.TotalMilliseconds).ToString(CultureInfo.InvariantCulture)
                        }
                    },
                    headers: new Dictionary<string, object>
                    {
                        {
                            "Accept", WellKnownMimeContentTypes.ApplicationJson
                        }
                    },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Accepted:
                {
                    var partialVerification = await response
                        .Content
                        .DeserializeAsync<PartialVerification>(restClient)
                        .ConfigureAwait(false);

                    // Return immediately if the email verification has been completed or if we should not wait for it

                    if (waitOptionsOrDefault == WaitOptions.NoWait || partialVerification.Overview.Status == VerificationStatus.Completed)
                    {
                        return await RetrieveVerificationFromPartialVerificationAsync(partialVerification, cancellationToken)
                            .ConfigureAwait(false);
                    }

                    return await WaitForCompletionAsync<Verification>(partialVerification.Overview,
                            waitOptionsOrDefault,
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

                    throw await restClient
                        .BuildRequestFailedExceptionAsync(response, cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }

        private async Task<Verification> RetrieveVerificationFromPartialVerificationAsync(PartialVerification partialVerification, CancellationToken cancellationToken)
        {
            if (partialVerification == null) throw new ArgumentNullException(nameof(partialVerification));

            var allEntries = new List<VerificationEntry>(partialVerification.Overview.NoOfEntries);
            var currentSegment = partialVerification.Entries;

            while (currentSegment?.Data != null)
            {
                allEntries.AddRange(currentSegment.Data);

                if (!(currentSegment.Meta?.IsTruncated ?? false))
                {
                    break;
                }

                var listingCursor = new ListingCursor(currentSegment.Meta.Cursor);

                currentSegment = await GetEntriesPageAsync(partialVerification.Overview.Id,
                        listingCursor,
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            return new Verification
            {
                Overview = partialVerification.Overview,
                Entries = allEntries
            };
        }

        public async Task<VerificationOverview?> GetOverviewAsync(string verificationId, WaitOptions? waitOptions = null, CancellationToken cancellationToken = default)
        {
            var waitOptionsOrDefault = waitOptions ?? WaitOptions.Default;
            
            // Send the request to the Verifalia servers

            var restClient = _restClientFactory.Build();

            using var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    $"email-validations/{verificationId}/overview",
                    queryParams: new Dictionary<string, string>
                    {
                        {
                            "waitTime", ((int) waitOptionsOrDefault.PollWaitTime.TotalMilliseconds).ToString(CultureInfo.InvariantCulture)
                        }
                    },
                    headers: new Dictionary<string, object>
                    {
                        {
                            "Accept", WellKnownMimeContentTypes.ApplicationJson
                        }
                    },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Accepted:
                {
                    var verificationOverview = await response
                        .Content
                        .DeserializeAsync<VerificationOverview>(restClient)
                        .ConfigureAwait(false);

                    verificationOverview.Status = response.StatusCode == HttpStatusCode.Accepted
                        ? VerificationStatus.InProgress
                        : VerificationStatus.Completed;

                    // Return immediately if the email verification has been completed or if we should not wait for it

                    if (waitOptions == WaitOptions.NoWait || verificationOverview.Status == VerificationStatus.Completed)
                    {
                        return verificationOverview;
                    }

                    return await WaitForCompletionAsync<VerificationOverview>(verificationOverview,
                            waitOptionsOrDefault,
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

            throw await restClient
                .BuildRequestFailedExceptionAsync(response, cancellationToken)
                .ConfigureAwait(false);
        }


#if HAS_ASYNC_ENUMERABLE_SUPPORT

        public IAsyncEnumerable<VerificationEntry> ListEntriesAsync(string verificationId, VerificationEntryListingOptions? options = null, CancellationToken cancellationToken = default)
        {
            return AsyncEnumerableHelper
                .ToAsyncEnumerableAsync<VerificationEntryPagedResult, VerificationEntry, VerificationEntryListingOptions>(
                    (listingOptions, token) => GetEntriesPageAsync(verificationId, listingOptions, token),
                    (cursor, token) => GetEntriesPageAsync(verificationId, cursor, token),
                    options,
                    cancellationToken);
        }

#endif

        public async Task<VerificationEntryPagedResult> GetEntriesPageAsync(string verificationId, VerificationEntryListingOptions? options = null, CancellationToken cancellationToken = default)
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

                // Predicates

                if (options.StatusFilter != null)
                {
                    foreach (var fragment in options.StatusFilter.Serialize("status"))
                    {
                        queryParams[fragment.Key] = fragment.Value;
                    }
                }
            }

            using var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    $"email-validations/{verificationId}/entries",
                    queryParams: queryParams,
                    headers: new Dictionary<string, object> { { "Accept", WellKnownMimeContentTypes.ApplicationJson } },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            
            return await GetEntriesPageImplAsync(restClient, response, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<VerificationEntryPagedResult> GetEntriesPageAsync(string verificationId, ListingCursor cursor, CancellationToken cancellationToken = default)
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
                    $"email-validations/{verificationId}/entries",
                    queryParams,
                    headers: new Dictionary<string, object> { { "Accept", WellKnownMimeContentTypes.ApplicationJson } },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            
            return await GetEntriesPageImplAsync(restClient, response, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<VerificationEntryPagedResult> GetEntriesPageImplAsync(IRestClient restClient, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response
                    .Content
                    .DeserializeAsync<VerificationEntryPagedResult>(restClient)
                    .ConfigureAwait(false);
            }

            throw await restClient
                .BuildRequestFailedExceptionAsync(response, cancellationToken)
                .ConfigureAwait(false);
        }
        
        public async Task<Stream> ExportEntriesAsync(string verificationId, ExportedEntriesFormat format, VerificationEntryListingOptions? options = null, CancellationToken cancellationToken = default)
        {
            // Determine the acceptable MIME content type

            var acceptableMimeContentType = format switch
            {
                ExportedEntriesFormat.Csv => WellKnownMimeContentTypes.TextCsv,
                ExportedEntriesFormat.ExcelXls => WellKnownMimeContentTypes.ExcelXls,
                ExportedEntriesFormat.ExcelXlsx => WellKnownMimeContentTypes.ExcelXlsx,
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            };

            // Send the request to the Verifalia servers

            var restClient = _restClientFactory.Build();

            var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    $"email-validations/{verificationId}/entries",
                    headers: new Dictionary<string, object> { { "Accept", acceptableMimeContentType } },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            
            // On success, return the response body (which is formatted according to the requested export format)

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response
                    .Content
#if NET5_0_OR_GREATER
                    .ReadAsStreamAsync(cancellationToken)
#else
                    .ReadAsStreamAsync()
#endif
                    .ConfigureAwait(false);
            }

            // An unexpected HTTP status code has been received at this point

            throw await restClient
                .BuildRequestFailedExceptionAsync(response, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
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
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Common;
using Verifalia.Api.Common.Models;
using Verifalia.Api.ContactMethods.Models;

namespace Verifalia.Api.ContactMethods
{
    /// <inheritdoc />
    internal sealed partial class ContactMethodsClient : IContactMethodsClient
    {
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        public IAsyncEnumerable<ContactMethod> ListAsync(string userId, ContactMethodListingOptions? options = null, CancellationToken cancellationToken = default)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            
            return AsyncEnumerableHelper
                .ToAsyncEnumerableAsync<ContactMethodListSegment, ContactMethod, ContactMethodListingOptions>(
                    FetchFirstSegment, 
                    FetchNextSegment,
                    options,
                    cancellationToken);

            Task<ContactMethodListSegment> FetchFirstSegment(ContactMethodListingOptions? options = null, CancellationToken cancellationToken = default)
            {
                return ListSegmentedAsync(userId, options, cancellationToken);
            }
            
            Task<ContactMethodListSegment> FetchNextSegment(ListingCursor cursor, CancellationToken cancellationToken = default)
            {
                return ListSegmentedAsync(userId, cursor, cancellationToken);
            }
        }
#endif

        public async Task<ContactMethodListSegment> ListSegmentedAsync(string userId, ContactMethodListingOptions? options = null, CancellationToken cancellationToken = default)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            
            // Generate the additional parameters, where needed

            var restClient = _restClientFactory.Build();

            Dictionary<string, string>? queryParams = null;

            if (options != null)
            {
                queryParams = new Dictionary<string, string>();

                // Standard parameters

                if (options.Limit > 0)
                {
                    queryParams["limit"] = options.Limit.ToString(CultureInfo.InvariantCulture);
                }
            }

            // Send the request to the Verifalia servers
            
            return await ListSegmentedImplAsync(restClient, userId, queryParams, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ContactMethodListSegment> ListSegmentedAsync(string userId, ListingCursor cursor, CancellationToken cancellationToken = default)
        {
            if (cursor == null) throw new ArgumentNullException(nameof(cursor));

            // Generate the additional parameters, where needed

            var restClient = _restClientFactory.Build();

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

            // Send the request to the Verifalia servers
            
            return await ListSegmentedImplAsync(restClient, userId, queryParams, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<ContactMethodListSegment> ListSegmentedImplAsync(IRestClient restClient, string userId, Dictionary<string, string>? queryParams, CancellationToken cancellationToken)
        {
            if (restClient == null) throw new ArgumentNullException(nameof(restClient));
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            
            using var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    $"users/{userId}/contact-methods",
                    queryParams,
                    headers: new Dictionary<string, object>
                    {
                        {
                            "Accept", $"{WellKnownMimeContentTypes.ApplicationJson}, {WellKnownMimeContentTypes.ApplicationProblemJson}"
                        }
                    },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response
                    .Content
                    .DeserializeAsync<ContactMethodListSegment>(restClient)
                    .ConfigureAwait(false);
            }
            
            // Unexpected error

            throw await restClient
                .BuildRequestFailedExceptionAsync(response, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
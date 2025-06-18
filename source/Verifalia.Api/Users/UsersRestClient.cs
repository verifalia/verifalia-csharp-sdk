/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2024 Cobisi Research
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
using Verifalia.Api.Users.Models;

namespace Verifalia.Api.Users
{
    /// <inheritdoc />
    internal sealed class UsersRestClient : IUsersRestClient
    {
        private readonly IRestClientFactory _restClientFactory;

        internal UsersRestClient(IRestClientFactory restClientFactory)
        {
            _restClientFactory = restClientFactory ?? throw new ArgumentNullException(nameof(restClientFactory));
        }

#if HAS_ASYNC_ENUMERABLE_SUPPORT

        public IAsyncEnumerable<UserOverview> ListUsersAsync(UserListingOptions? options = null, CancellationToken cancellationToken = default)
        {
            return AsyncEnumerableHelper
                .ToAsyncEnumerableAsync<UserListSegment, UserOverview, UserListingOptions>(
                    ListUsersSegmentedAsync,
                    ListUsersSegmentedAsync,
                    options,
                    cancellationToken);
        }

#endif

        public async Task<UserListSegment> ListUsersSegmentedAsync(UserListingOptions? options = null, CancellationToken cancellationToken = default)
        {
            // Generate the additional parameters, where needed

            var restClient = _restClientFactory.Build();

            // Send the request to the Verifalia servers

            Dictionary<string, string>? queryParams = null;

            if (options != null)
            {
                queryParams = new Dictionary<string, string>();

                // Standard parameters

                if (options.Limit > 0)
                {
                    queryParams["limit"] = options.Limit.ToString(CultureInfo.InvariantCulture);
                }

                // Predicates

                if (options.IncludeDeleted)
                {
                    queryParams["includeDeleted"] = "true";
                }
                
                if (options.UserTypeFilter != null)
                {
                    foreach (var fragment in options.UserTypeFilter.Serialize("type"))
                    {
                        queryParams[fragment.Key] = fragment.Value;
                    }
                }
                
                // Sort

                switch (options.OrderBy)
                {
                    case UserListingField.CreatedOn:
                        queryParams["sort"] = $"{(options.Direction == Direction.Backward ? "-" : null)}createdOn";
                        break;

                    case UserListingField.DisplayName:
                        queryParams["sort"] = $"{(options.Direction == Direction.Backward ? "-" : null)}displayName";
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException(nameof(options));
                }
            }

            return await ListUsersSegmentedImplAsync(restClient, queryParams, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<UserListSegment> ListUsersSegmentedAsync(ListingCursor cursor, CancellationToken cancellationToken = default)
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

            return await ListUsersSegmentedImplAsync(restClient, queryParams, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<UserListSegment> ListUsersSegmentedImplAsync(IRestClient restClient, Dictionary<string, string>? queryParams, CancellationToken cancellationToken)
        {
            using var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    "users",
                    queryParams,
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
                {
                    return await response
                        .Content
                        .DeserializeAsync<UserListSegment>(restClient)
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

                    throw new VerifaliaException($"Unexpected HTTP response: {(int)response.StatusCode} {responseBody}");
                }
            }
        }
    }
}
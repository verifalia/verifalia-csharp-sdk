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
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Credits.Models;
using Verifalia.Api.Common.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Verifalia.Api.Credits
{
    /// <summary>
    /// Interface defining methods for interacting with credits balance and usage consumption in the Verifalia API.
    /// </summary>
    public interface ICreditsClient
    {
        /// <summary>
        /// Retrieves the current Verifalia account balance.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The balance of the credits, including available credit packs and free credits.</returns>
        Task<Balance> GetBalanceAsync(CancellationToken cancellationToken = default);

#if HAS_ASYNC_ENUMERABLE_SUPPORT
        /// <summary>
        /// Retrieves the daily usage of credits for the Verifalia account.
        /// </summary>
        /// <param name="options">Options for filtering and sorting the results. Use null to retrieve all records.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        IAsyncEnumerable<DailyUsage> ListDailyUsagesAsync(DailyUsageListingOptions? options = null, CancellationToken cancellationToken = default);
#endif

        /// <summary>
        /// Retrieves the first page of daily usage of credits for the Verifalia account.
        /// </summary>
        /// <param name="options">Options for filtering and sorting the results; use null to retrieve all records.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListDailyUsageAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<DailyUsagePagedResult> GetDailyUsagesPageAsync(DailyUsageListingOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a page of daily usage of credits for the Verifalia account.
        /// </summary>
        /// <param name="cursor">A cursor object containing pagination information to fetch this page.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
#if HAS_ASYNC_ENUMERABLE_SUPPORT
        [Obsolete("ListDailyUsageAsync() is preferred in .NET Core 3.0+ because of its simpler syntax, thanks to the async enumerable support.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        Task<DailyUsagePagedResult> GetDailyUsagesPageAsync(ListingCursor cursor, CancellationToken cancellationToken = default);
    }
}
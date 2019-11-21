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

#if HAS_ASYNC_ENUMERABLE_SUPPORT

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Common.Models;

namespace Verifalia.Api.Common
{
    /// <summary>
    /// Internal helper for .NET Core 3+ IAsyncEnumerable support.
    /// </summary>
    internal static class AsyncEnumerableHelper
    {
        internal static async IAsyncEnumerable<TItem> ToAsyncEnumerable<TList, TItem, TOptions>(
            Func<TOptions, CancellationToken, Task<TList>> fetchFirstSegment,
            Func<ListingCursor, CancellationToken, Task<TList>> fetchNextSegment, TOptions options = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
            where TOptions : ListingOptions
            where TList : ListSegment<TItem>
        {
            ListingCursor cursor = null;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Retrieve the first (or its subsequent) result list segment

                TList segment;

                if (cursor == null)
                {
                    segment = await fetchFirstSegment(options, cancellationToken)
                        .ConfigureAwait(false);
                }
                else
                {
                    segment = await fetchNextSegment(cursor, cancellationToken)
                        .ConfigureAwait(false);
                }

                // Iterate the result collection

                foreach (var item in segment.Data)
                {
                    yield return item;
                }

                // Stop processing if this is the last segment

                if (!segment.Meta.IsTruncated)
                {
                    break;
                }

                // Build the cursor for the next iteration

                cursor = new ListingCursor(segment.Meta.Cursor);

                if (options != null)
                {
                    cursor.Limit = options.Limit;
                    cursor.Direction = options.Direction;
                };
            }
        }
    }
}

#endif
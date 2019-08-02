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

namespace Verifalia.Api.Common.Models
{
    /// <summary>
    /// The options for a listing operation against the Verifalia API.
    /// </summary>
    public class ListingOptions
    {
        private int _limit;

        /// <summary>
        /// The direction of the listing.
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// The maximum number of items to return with a listing request.
        /// <remarks>The Verifalia API may choose to override the specified limit if it is either too small or too big.</remarks>
        /// <remarks>Under async enumerable methods available in this SDK on .NET Core 3.0+, a single listing operations
        /// may automatically perform different listing requests to the Verifalia API: this value limits the number of items
        /// returned by each API request, not the overall total number of returned items. To limit the total number of returned
        /// items, use one of the LINQ methods exposed by the <a href="https://www.nuget.org/packages/System.Interactive/">System.Interactive</a>
        /// package, like Take().</remarks>
        /// </summary>
        public int Limit
        {
            get => _limit;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Limit must be 0 (meaning no limit will be enforced) or greater.");
                }

                _limit = value;
            }
        }
    }
}
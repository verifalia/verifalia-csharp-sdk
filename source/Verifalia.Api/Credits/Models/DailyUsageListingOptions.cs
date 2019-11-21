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

using Verifalia.Api.Filters;
using Verifalia.Api.Common.Models;

namespace Verifalia.Api.Credits.Models
{
    /// <summary>
    /// The options for a daily usage listing operation against the Verifalia API.
    /// </summary>
    public class DailyUsageListingOptions : ListingOptions
    {
        /// <summary>
        /// If set, apply a filter against the daily usage dates, before returning their usage records. Use  either <see cref="DateEqualityPredicate"/>
        /// to specify an exact date to match or <see cref="DateBetweenPredicate"/> to set a range of dates.
        /// </summary>
        public DateFilterPredicate DateFilter { get; set; }
    }
}
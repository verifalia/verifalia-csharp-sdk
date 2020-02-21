/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2020 Cobisi Research
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
using System.Collections.Generic;

namespace Verifalia.Api.Filters
{
    /// <summary>
    /// A filter predicate used to filter dates between two optional values.
    /// </summary>
    /// <inheritdoc />
    public sealed class DateBetweenPredicate : DateFilterPredicate
    {
        /// <summary>
        /// The minimum date to be included in the filter.
        /// </summary>
        public DateTime? Since { get; set; }

        /// <summary>
        /// The maximum date to be included in the filter.
        /// </summary>
        public DateTime? Until { get; set; }

        public override IEnumerable<FilterPredicateFragment> Serialize(string fieldName)
        {
            if (Since != null)
            {
                yield return new FilterPredicateFragment($"{fieldName}:since", $"{Since:yyyy-MM-dd}");
            }

            if (Until != null)
            {
                yield return new FilterPredicateFragment($"{fieldName}:until", $"{Until:yyyy-MM-dd}");
            }
        }
    }
}
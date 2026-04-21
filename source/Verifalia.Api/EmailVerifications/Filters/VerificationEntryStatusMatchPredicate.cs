/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2026 Cobisi Research
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
using System.Linq;
using Verifalia.Api.EmailVerifications.Models;
using Verifalia.Api.EmailVerifications.Converters;
using Verifalia.Api.Filters;

namespace Verifalia.Api.EmailVerifications.Filters
{
    /// <summary>
    /// A predicate that filters verification entries by their <see cref="VerificationEntry.Status"/>, matching a list
    /// of included and excluded statuses.
    /// </summary>
    /// <inheritdoc />
    public class VerificationEntryStatusMatchPredicate : FilterPredicate
    {
        /// <summary>
        /// Specifies the verification entry status values to include in the filter.
        /// </summary>
        public VerificationEntryStatus[]? IncludedValues { get; set; }

        /// <summary>
        /// Specifies the verification entry statuses that should be excluded from matching.
        /// </summary>
        public VerificationEntryStatus[]? ExcludedValues { get; set; }

        public override IEnumerable<FilterPredicateFragment> Serialize(string fieldName)
        {
            if (IncludedValues != null && IncludedValues.Length > 0)
            {
                yield return new FilterPredicateFragment($"{fieldName}:include",
                    String.Join(",",
                        IncludedValues.Select(v => VerificationEntryStatusConverter.Mappings.First(kvp => kvp.Value == v).Key)));
            }

            if (ExcludedValues != null && ExcludedValues.Length > 0)
            {
                yield return new FilterPredicateFragment($"{fieldName}:exclude",
                    String.Join(",",
                        ExcludedValues.Select(v => VerificationEntryStatusConverter.Mappings.First(kvp => kvp.Value == v).Key)));
            }
        }
    }
}
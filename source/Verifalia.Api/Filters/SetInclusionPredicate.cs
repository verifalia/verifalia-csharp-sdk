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

namespace Verifalia.Api.Filters
{
    /// <summary>
    /// A filter predicate used to include certain elements from a target set of possible values.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    public sealed class SetInclusionPredicate<T> : SetFilterPredicate<T>
    {
        private readonly T[] _values;

        /// <summary>
        /// The values to include from the target set.
        /// </summary>
        public IReadOnlyCollection<T> Values => _values;

        /// <summary>
        /// Initializes a filter predicate used to include certain elements from a target set of possible values.
        /// </summary>
        /// <param name="values">The values to include from the target set.</param>
        public SetInclusionPredicate(params T[] values)
        {
            _values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public override IEnumerable<FilterPredicateFragment> Serialize(string fieldName)
        {
            if (_values.Length > 0)
            {
                return new FilterPredicateFragment[]
                {
                    new(fieldName, String.Join(",", _values))
                };
            }

#if NETFRAMEWORK
            return new FilterPredicateFragment[0];
#else
            return Array.Empty<FilterPredicateFragment>();
#endif
        }
    }
}
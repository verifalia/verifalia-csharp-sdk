/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2020 Cobisi Research
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

using System.Collections.Generic;

namespace Verifalia.Api.Filters
{
    /// <summary>
    /// Base class for filter predicates, used to filter data retrieved from the Verifalia API.
    /// </summary>
    public abstract class FilterPredicate
    {
        /// <summary>
        /// Provides a sequence of <see cref="FilterPredicateFragment"/> raw key-value pairs, to be passed to
        /// the Verifalia API for filtering data.
        /// </summary>
        /// <param name="fieldName">The name of the field accepted by the Verifalia API.</param>
        /// <returns>A sequence of <see cref="FilterPredicateFragment"/> raw key-value pairs, to be passed to the
        /// Verifalia API for filtering data.</returns>
        public abstract IEnumerable<FilterPredicateFragment> Serialize(string fieldName);
    }
}
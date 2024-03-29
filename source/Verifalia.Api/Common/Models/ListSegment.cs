﻿/*
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

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Verifalia.Api.Common.Models
{
    /// <summary>
    /// A segment of a list of <see cref="TItem"/>, returned by a Verifalia API which supports key-set navigation.
    /// </summary>
    /// <typeparam name="TItem">The type of items of this list.</typeparam>
    public abstract class ListSegment<TItem>
    {
        /// <summary>
        /// The meta-data for this list segment.
        /// </summary>
        [JsonProperty("meta")]
        public ListSegmentMeta? Meta { get; set; }

        /// <summary>
        /// The items of type <see cref="TItem"/> included in this segment.
        /// </summary>
        [JsonProperty("data")]
        public IReadOnlyList<TItem> Data { get; set; }
    }
}
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

using System;
using Newtonsoft.Json;

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// Progress details for a <see cref="Validation.Overview"/>, exposed by way of the <see cref="ValidationOverview.Progress"/> property.
    /// </summary>
    public class ValidationProgress
    {
        /// <summary>
        /// The percentage of completed entries, ranging from 0 to 1.
        /// </summary>
        [JsonProperty("percentage", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Percentage { get; set; }

        /// <summary>
        /// An eventual estimated required time span needed to complete the whole job.
        /// </summary>
        [JsonProperty("estimatedTimeRemaining", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public TimeSpan? EstimatedTimeRemaining { get; set; }
    }
}
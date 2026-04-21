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

using Newtonsoft.Json;

namespace Verifalia.Api.Users.Models
{
    /// <summary>
    /// An object representing a throttling rule. 
    /// </summary>
    public sealed class ThrottlingRule
    {
        /// <summary>
        /// Represents the maximum number of jobs allowed during the specified period.
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Indicates the time window for the limit. Accepted values are <see cref="ThrottlingPeriod.Minute"/>,
        /// <see cref="ThrottlingPeriod.Hour"/>, or <see cref="ThrottlingPeriod.Day"/>.
        /// </summary>
        [JsonProperty("period")]
        public ThrottlingPeriod Period { get; set; }
        
        /// <summary>
        /// Defines the rule’s scope. Accepted values are <see cref="ThrottlingScope.Global"/> or <see cref="ThrottlingScope.IPAddress"/>.
        /// </summary>
        [JsonProperty("scope")]
        public ThrottlingScope Scope { get; set; }
    }
}
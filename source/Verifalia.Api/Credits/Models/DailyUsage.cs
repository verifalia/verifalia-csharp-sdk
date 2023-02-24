/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2023 Cobisi Research
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
using Verifalia.Api.Common.Converters;

namespace Verifalia.Api.Credits.Models
{
    /// <summary>
    /// A total usage of Verifalia credits along a specific day.
    /// </summary>
    public class DailyUsage
    {
        /// <summary>
        /// The date this credits usage refers to.
        /// </summary>
        [JsonProperty("date")]
        [JsonConverter(typeof(Iso8601DateConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// The number of free daily credits used during the day.
        /// </summary>
        /// <remarks>Free daily credits depend on the plan of your Verifalia account; visit https://verifalia.com/client-area#/account/change-plan
        /// to change your plan.</remarks>
        [JsonProperty("freeCredits")]
        public decimal FreeCredits { get; set; }

        /// <summary>
        /// The number of credit packs used during the day.
        /// </summary>
        /// <remarks>Visit https://verifalia.com/client-area#/credits/add to add credit packs to your Verifalia account.</remarks>
        [JsonProperty("creditPacks")]
        public decimal CreditPacks { get; set; }
    }
}
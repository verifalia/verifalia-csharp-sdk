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
using Newtonsoft.Json;

namespace Verifalia.Api.Credits.Models
{
    /// <summary>
    /// The credits balance for the Verifalia account.
    /// </summary>
    public class Balance
    {
        /// <summary>
        /// The number of credit packs (that is, non-expiring credits) available for the account.
        /// </summary>
        /// <remarks>Visit https://verifalia.com/client-area#/credits/add to add credit packs to your Verifalia account.</remarks>
        [JsonProperty("creditPacks")]
        public decimal CreditPacks { get; set; }

        /// <summary>
        /// The number of free daily credits of the account.
        /// </summary>
        /// <remarks>Free daily credits depend on the plan of your Verifalia account; visit https://verifalia.com/client-area#/account/change-plan
        /// to change your plan.</remarks>
        [JsonProperty("freeCredits")]
        public decimal? FreeCredits { get; set; }

        /// <summary>
        /// The time required for the free daily credits to reset.
        /// </summary>
        [JsonProperty("freeCreditsResetIn")]
        public TimeSpan? FreeCreditsResetIn { get; set; }
    }
}

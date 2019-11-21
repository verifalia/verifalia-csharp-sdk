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

using System;
using Newtonsoft.Json;

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// The details of a quality level.
    /// </summary>
    public class QualityLevel
    {
        /// <summary>
        /// The ID of the quality level.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// A text used to display this quality level to final users.
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// If true, this quality level is the default for the account.
        /// </summary>
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// If true, this quality level is customized with exclusive settings for the account.
        /// </summary>
        [JsonProperty("isCustom")]
        public bool IsCustom { get; set; }

        /// <summary>
        /// The unit price for an email validation submitted using this quality level, expressed in Verifalia credits.
        /// </summary>
        /// <remarks>This value can be null in the event the calling user does not have enough permissions.</remarks>
        [JsonProperty("unitPrice")]
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// If true, this quality level is unavailable for the account plan.
        /// </summary>
        [JsonProperty("isUnavailable")]
        public bool IsUnavailable { get; set; }

        /// <summary>
        /// A <see cref="QualityLevelName"/> to be used as a reference while calling the Verifalia API.
        /// </summary>
        [JsonIgnore]
        public QualityLevelName Name => new QualityLevelName(Id);
    }
}
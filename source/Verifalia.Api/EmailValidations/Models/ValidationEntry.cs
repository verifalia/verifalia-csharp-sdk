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

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// Represents a single validated entry within a <see cref="Validation"/>.
    /// </summary>
    public class ValidationEntry
    {
        /// <summary>
        /// The index of this entry within its <see cref="Validation"/> container. This property is mostly useful in the event
        /// the API returns a filtered view of the items.
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

        /// <summary>
        /// The input string being validated.
        /// </summary>
        [JsonProperty("inputData")]
        public string InputData { get; set; }

        /// <summary>
        /// A custom, optional string which is passed back upon completing the validation. To pass back and forth a custom value,
        /// use the <see cref="ValidationRequestEntry.Custom"/> property of <see cref="ValidationRequestEntry"/>.
        /// </summary>
        [JsonProperty("custom")]
        public string? Custom { get; set; }

        /// <summary>
        /// The date this entry has been completed, if available.
        /// </summary>
        [JsonProperty("completedOn")]
        public DateTime? CompletedOn { get; set; }

        /// <summary>
        /// Gets the email address, without any eventual comment or folding white space. Returns null if the input data
        /// is not a syntactically invalid e-mail address.
        /// </summary>
        [JsonProperty("emailAddress")]
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Gets the domain part of the email address, converted to ASCII if needed and with comments and folding
        /// white spaces stripped off.
        /// </summary>
        /// <remarks>The ASCII encoding is performed using the standard <see cref="http://en.wikipedia.org/wiki/Punycode">punycode algorithm</see>.</remarks>
        /// <remarks>To get the domain part without any ASCII encoding, use <see cref="EmailAddressDomainPart"/>.</remarks>
        [JsonProperty("asciiEmailAddressDomainPart")]
        public string? AsciiEmailAddressDomainPart { get; set; }

        /// <summary>
        /// Gets the local part of the email address, without comments and folding white spaces.
        /// </summary>
        [JsonProperty("emailAddressLocalPart")]
        public string? EmailAddressLocalPart { get; set; }

        /// <summary>
        /// Gets the domain part of the email address, without comments and folding white spaces.
        /// </summary>
        /// <remarks>If the ASCII-only (punycode) version of the domain part is needed, use <see cref="AsciiEmailAddressDomainPart"/>.</remarks>
        [JsonProperty("emailAddressDomainPart")]
        public string? EmailAddressDomainPart { get; set; }

        /// <summary>
        /// If true, the email address has an international domain name.
        /// </summary>
        [JsonProperty("hasInternationalDomainName")]
        public bool? HasInternationalDomainName { get; set; }

        /// <summary>
        /// If true, the email address has an international mailbox name.
        /// </summary>
        [JsonProperty("hasInternationalMailboxName")]
        public bool? HasInternationalMailboxName { get; set; }

        /// <summary>
        /// If true, the email address comes from a disposable email address (DEA) provider.
        /// See <see cref="https://verifalia.com/help/email-validations/what-is-a-disposable-email-address-dea"/> for additional information
        /// about disposable email addresses.
        /// </summary>
        [JsonProperty("isDisposableEmailAddress")]
        public bool? IsDisposableEmailAddress { get; set; }

        /// <summary>
        /// If true, the email address comes from a free email address provider (e.g. gmail, yahoo, outlook / hotmail, ...).
        /// </summary>
        [JsonProperty("isFreeEmailAddress")]
        public bool? IsFreeEmailAddress { get; set; }

        /// <summary>
        /// If true, the local part of the email address is a well-known role account.
        /// </summary>
        [JsonProperty("isRoleAccount")]
        public bool? IsRoleAccount { get; set; }

        /// <summary>
        /// The validation status for this entry.
        /// </summary>
        [JsonProperty("status")]
        public ValidationEntryStatus Status { get; set; }

        /// <summary>
        /// The <see cref="ValidationEntryClassification"/> for the status of this email address.
        /// </summary>
        /// <remarks>Standard values include <see cref="ValidationEntryClassification.Deliverable"/>, <see cref="ValidationEntryClassification.Risky"/>,
        /// <see cref="ValidationEntryClassification.Undeliverable"/> and <see cref="ValidationEntryClassification.Unknown"/>.</remarks>
        [JsonProperty("classification")]
        public ValidationEntryClassification Classification { get; set; }

        /// <summary>
        /// The position of the character in the email address that eventually caused the syntax validation to fail.
        /// </summary>
        /// <remarks>Returns <see langword="null">null</see> if there isn't any syntax failure.</remarks>
        [JsonProperty("syntaxFailureIndex")]
        public int? SyntaxFailureIndex { get; set; }

        /// <summary>
        /// The zero-based index of the first occurrence of this email address in the parent <see cref="Validation"/>, in the event the <see cref="Status"/>
        /// for this entry is <see cref="ValidationEntryStatus.Duplicate"/>; duplicated items do not expose any result detail apart from this and the
        /// eventual <see cref="Custom"/> values.
        /// </summary>
        [JsonProperty("duplicateOf")]
        public int? DuplicateOf { get; set; }
        
        /// <summary>
        /// The potential corrections for the input data, in the event Verifalia identified potential typos during the verification process.
        /// </summary>
        [JsonProperty("suggestions")]
        public string[]? Suggestions { get; set; }
    }
}

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
using Newtonsoft.Json;
using Verifalia.Api.ContactMethods.Converters;

namespace Verifalia.Api.ContactMethods.Models
{
    /// <summary>
    /// Represents a contact method used by Verifalia to send notifications for the owning user; notifications include
    /// system alerts, commercial document updates, and news.
    /// </summary>
    public sealed class ContactMethod
    {
        /// <summary>
        /// A server-generated unique identifier for the contact method.
        /// </summary>
        /// <remarks>This property is ignored when creating a new contact method.</remarks>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// A hash that changes whenever the contact method's settings are modified, useful for caching and optimistic
        /// concurrency control.
        /// </summary>
        /// <remarks>This property is ignored when creating a new contact method.</remarks>
        [JsonProperty("etag")]
        public string Etag { get; set; }

        /// <summary>
        /// Indicates the type of contact method.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(ContactMethodTypeConverter))]
        public ContactMethodType Type { get; set; }

        /// <summary>
        /// Indicates the current status of the contact method.
        /// </summary>
        /// <remarks>This property is ignored when creating a new contact method.</remarks>
        [JsonProperty("status")]
        [JsonConverter(typeof(ContactMethodStatusConverter))]
        public ContactMethodStatus Status { get; set; }

        /// <summary>
        /// The timestamp when the contact method was created in Verifalia.
        /// </summary>
        /// <remarks>This property is ignored when creating a new contact method.</remarks>
        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The timestamp when the contact method was activated; if null, the contact method has not been activated.
        /// </summary>
        /// <remarks>This property is ignored when creating a new contact method.</remarks>
        [JsonProperty("activatedOn")]
        public DateTime? ActivatedOn { get; set; }

        /// <summary>
        /// A human-friendly label for the contact method. This text is used in the Verifalia client area for management
        /// purposes and appears in an email’s header to help identify the recipient.
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        
        /// <summary>
        /// The email address associated with the contact method (only if <see cref="Type"/> is <see cref="ContactMethodType.Email"/>).
        /// </summary>
        [JsonProperty("emailAddress")]
        public string? EmailAddress { get; set; }
    }
}
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
    /// Represents an overview of a Verifalia user. 
    /// </summary>
    public sealed class UserOverview
    {
        /// <summary>
        /// Contains the user’s display name, typically their full name.
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Contains a hash that changes when the user's settings change, allowing for efficient caching and optimistic
        /// concurrency control.
        /// </summary>
        [JsonProperty("etag")]
        public string Etag { get; set; }

        /// <summary>
        /// The unique identifier of the user.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// A flag that indicates whether the user is currently active; inactive users cannot access Verifalia but still
        /// count toward the account’s user limit.
        /// </summary>
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Shows whether the user has been permanently deleted or not.
        /// </summary>
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
        
        /// <summary>
        /// Specifies the type of user. Accepted values are:
        /// <ul>
        /// <li><see cref="UserType.Administrator"/> for account administrators;</li>
        /// <li><see cref="UserType.Standard"/> for standard users;</li>
        /// <li><see cref="UserType.BrowserApp"/> for browser apps;</li>
        /// </ul>
        /// </summary>
        [JsonProperty("type")]
        public UserType Type { get; set; }
    }
}
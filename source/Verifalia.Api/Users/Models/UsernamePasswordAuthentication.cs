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
    /// Contains the settings related to username-password authentication. Passwordless authentication is
    /// treated as username-password authentication with an empty password.
    /// </summary>
    public sealed class UsernamePasswordAuthentication
    {
        /// <summary>
        /// When set to true, the user can authenticate using a username and password.
        /// </summary>
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// A flag that controls whether multi-factor authentication (MFA) is required when using username-password authentication.
        /// </summary>
        [JsonProperty("isMfaRequired")]
        public bool IsMfaRequired { get; set; }
        
        /// <summary>
        /// Represents the user's username; for browser apps, this is the publishable browser app key. If username-password
        /// authentication is not enabled, this field may be omitted.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }
        
        /// <summary>
        /// The password of the user.
        /// </summary>
        /// <remarks>This property is only used while creating or updating users.</remarks>
        [JsonProperty("password")]
        public string? Password { get; set; }
    }
}
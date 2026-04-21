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
    /// Includes the authentication methods available to the user.
    /// </summary>
    public sealed class AuthenticationSettings
    {
        /// <summary>
        /// Represents the settings related to X.509 client certificate authentication; applies only to standard users.
        /// </summary>
        [JsonProperty("certificate")]
        public ClientCertificateAuthentication? ClientCertificateAuthentication { get; set; }
        
        /// <summary>
        /// Contains the settings related to username-password authentication.
        /// </summary>
        [JsonProperty("password")]
        public UsernamePasswordAuthentication? UsernamePasswordAuthentication { get; set; }
        
        /// <summary>
        /// The ID of the contact method to be used for password recovery. If this field is omitted, the user does
        /// not have a configured recovery contact method and password recovery is unavailable.
        /// </summary>
        /// <remarks>To manage contact methods, use the methods exposed by <see cref="IVerifaliaClient.ContactMethods"/>.</remarks>
        [JsonProperty("recoveryContactMethod")]
        public string? RecoveryContactMethodId { get; set; }
    }
}
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
    /// Contains the complete settings of a Verifalia user. 
    /// </summary>
    public sealed class User
    {
        /// <summary>
        /// Lists the authentication methods available to the user.
        /// </summary>
        [JsonProperty("authentication")]
        public AuthenticationSettings? Authentication { get; set; }

        /// <summary>
        /// Includes the authorization settings for the user.
        /// </summary>
        [JsonProperty("authorization")]
        public AuthorizationSettings? Authorization { get; set; }

        /// <summary>
        /// Contains settings related to CAPTCHA enforcement for email verification requests made by the user.
        /// </summary>
        [JsonProperty("captcha")]
        public CaptchaSettings? Captcha { get; set; }
        
        /// <summary>
        /// Contains default configuration settings for the user.
        /// </summary>
        [JsonProperty("defaults")]
        public DefaultSettings? Defaults { get; set; }
        
        /// <summary>
        /// Contains the user’s display name, typically their full name.
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Contains settings related to API request firewall rules.
        /// </summary>
        [JsonProperty("firewall")]
        public FirewallSettings? Firewall { get; set; }
        
        /// <summary>
        /// A flag that indicates whether the user is currently active; inactive users cannot access Verifalia but still
        /// count toward the account’s user limit.
        /// </summary>
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Contains the ID of the user’s preferred contact method. If omitted, it means the user has no available contact
        /// methods; once the user adds their first contact method, the system automatically sets it as the preferred one.
        /// </summary>
        /// <remarks>To manage contact methods, use the methods exposed by <see cref="IVerifaliaClient.ContactMethods"/>.</remarks>
        [JsonProperty("preferredContactMethod")]
        public string? PreferredContactMethodId { get; set; }

        /// <summary>
        /// Contains rate-limiting settings for email verification jobs.
        /// </summary>
        [JsonProperty("throttling")]
        public ThrottlingSettings? Throttling { get; set; }

        /// <summary>
        /// Includes settings for trusted HTTP origins enforcement; applies only to browser apps.
        /// </summary>
        [JsonProperty("trustedOrigin")]
        public TrustedOriginSettings? TrustedOrigin { get; set; }
        
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
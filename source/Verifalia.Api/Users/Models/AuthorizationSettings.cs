/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2025 Cobisi Research
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
    /// Includes the authorization settings for the user.
    /// </summary>
    public sealed class AuthorizationSettings
    {
        /// <summary>
        /// An array of strings, each representing a specific authorization rule applied to the user.
        /// </summary>
        /// <remarks>Each authorization rule is represented by a string which follows this format:
        /// <code>[{effect}]resource[:operation[:scope]]</code>
        /// where:
        /// <ul>
        /// <li>effect can be either an empty string (which grants permission) or a dash - (which denies permission);</li>
        /// <li>resource[:operation[:scope]] is one of the permissions the API supports;</li>
        /// <li>if the scope is not specified, it's assumed to apply account-wide;</li>
        /// <li>similarly, if the operation is not specified, all operations are assumed.</li>
        /// </ul>
        /// You can use the <c>*</c> wildcard to match any value within a segment.
        /// For example: <c>email-verifications:*:own</c> means the user is granted all operations on the <c>email-verifications</c>
        /// resource, but only within their own scope.
        /// </remarks>
        [JsonProperty("rules")]
        public string[] Rules { get; set; }
    }
}
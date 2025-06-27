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

using System;
using Newtonsoft.Json;
using Verifalia.Api.EmailVerifications;

namespace Verifalia.Api.Users.Models
{
    /// <summary>
    /// Contains default configuration settings for a user.
    /// </summary>
    public sealed class DefaultSettings
    {
        /// <summary>
        /// Contains the default data retention period to observe for email verifications.
        /// Verifalia automatically deletes email verification data after the specified retention period, starting from
        /// the time the job is completed. If this value is set, its value overrides the default data retention period
        /// configured at the account level. You can also override both settings when submitting an individual
        /// verification job, if needed. Additionally, verification jobs can always be manually deleted before their
        /// retention period expires - see the <see cref="IEmailVerificationsClient.DeleteAsync(string,System.Threading.CancellationToken)"/> method of
        /// <see cref="IVerifaliaClient.EmailVerifications"/>.
        /// </summary>
        [JsonProperty("retention")]
        public TimeSpan? Retention { get; set; }
    }
}
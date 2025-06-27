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
    /// Contains CAPTCHA provider settings.
    /// </summary>
    public sealed class CaptchaProvidersSettings
    {
        /// <summary>
        /// Contains settings for integrating hCaptcha, omitted if hCaptcha is not configured. 
        /// </summary>
        [JsonProperty("hCaptcha")]
        public HCaptchaSettings? HCaptcha { get; set; }
        
        /// <summary>
        /// Contains settings for integrating Google reCAPTCHA v2, omitted if Google reCAPTCHA v2 is not configured.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        [JsonProperty("reCaptchaV2")]
        public ReCaptcha2Settings? ReCaptchaV2 { get; set; }
        
        /// <summary>
        /// Contains settings for integrating Google reCAPTCHA v3, omitted if Google reCAPTCHA v3 is not configured.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        [JsonProperty("reCaptchaV3")]
        public ReCaptcha3Settings? ReCaptchaV3 { get; set; }
        
        /// <summary>
        /// Contains settings for integrating Cloudflare Turnstile, omitted if Cloudflare Turnstile is not configured.
        /// </summary>
        [JsonProperty("turnstile")]
        public TurnstileSettings? Turnstile { get; set; }
    }
}
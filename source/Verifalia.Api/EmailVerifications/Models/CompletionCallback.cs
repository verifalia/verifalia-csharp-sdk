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

namespace Verifalia.Api.EmailVerifications.Models
{
    /// <summary>
    /// This class provides configuration settings for invoking a callback URI upon the completion of an email
    /// verification job.
    /// </summary>
    public sealed class CompletionCallback
    {
        private Uri _uri;

        /// <summary>
        /// The URL that Verifalia will invoke once the results for the email verification job are ready.
        /// </summary>
        public Uri Uri
        {
            get => _uri;
            set
            {
                EnsureValidCallbackUri(value);
                
                _uri = value;
            }
        }

        /// <summary>
        /// Allows setting a schema version for the callback. If not specified, the default schema version available
        /// in the target API version will be used.
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// Determines whether to skip server certificate validation when making the callback request. This is useful for
        /// testing purposes during development, especially when the callback server uses a self-signed certificate.
        /// </summary>
        public bool SkipServerCertificateValidation { get; set; }

        /// <summary>
        /// Represents a callback configuration for invoking a URI upon completion of an email verification job.
        /// </summary>
        public CompletionCallback(Uri uri)
        {
            EnsureValidCallbackUri(uri);

            Uri = uri;
        }

        private static void EnsureValidCallbackUri(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            if (!uri.IsAbsoluteUri || uri.Scheme is not ("https" or "http"))
            {
                throw new ArgumentOutOfRangeException(nameof(uri), "Callback must be an absolute https (or http) URI.");
            }
        }
    }
}
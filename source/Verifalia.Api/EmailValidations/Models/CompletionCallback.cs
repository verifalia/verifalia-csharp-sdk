/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2023 Cobisi Research
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

namespace Verifalia.Api.EmailValidations.Models
{
    public sealed class CompletionCallback
    {
        private Uri _uri;

        /// <summary>
        /// An URL which Verifalia will invoke once the results for the email verification job are ready.
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
        /// If set, allows to set a specific schema version which Verifalia will obey while invoking the callback. If
        /// unset, will use the default callback schema version available to the target API version.
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// If true, skips the server certificate validation for the external callback server, useful for testing purposes
        /// at development time when the callback server is using a self-signed certificate.
        /// </summary>
        public bool SkipServerCertificateValidation { get; set; }

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
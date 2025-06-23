/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2024 Cobisi Research
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

using Verifalia.Api.ContactMethods;
using Verifalia.Api.Credits;
using Verifalia.Api.EmailVerifications;
using Verifalia.Api.Users;

namespace Verifalia.Api
{
    /// <summary>
    /// HTTPS-based REST client for Verifalia.
    /// </summary>
    public interface IVerifaliaClient
    {
        /// <summary>
        /// Gets or sets the version of the Verifalia API to use when making requests; defaults to the latest API version supported
        /// by this SDK.
        /// <remarks>Warning: changing this value may affect the stability of the SDK itself.</remarks>
        /// </summary>
        string ApiVersion { get; set; }

        /// <summary>
        /// Allows to manage the contact methods of the Verifalia account.
        /// </summary>
        IContactMethodsRestClient ContactMethods { get; }
        
        /// <summary>
        /// Allows to manage the credits for the Verifalia account.
        /// </summary>
        ICreditsClient Credits { get; }

        /// <summary>
        /// Allows to verify email addresses and manage email verification jobs using the Verifalia service.
        /// </summary>
        IEmailVerificationsRestClient EmailVerifications { get; }

        /// <summary>
        /// Allows to manage the users and browser apps of the Verifalia account, as well as their security and configuration settings.
        /// </summary>
        IUsersRestClient Users { get; }
    }
}
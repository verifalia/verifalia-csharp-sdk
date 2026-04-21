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

using System;
using Newtonsoft.Json;

namespace Verifalia.Api.ClientCertificates.Models
{
    /// <summary>
    /// Represents an X.509 client certificate which can be used for TLS mutual authentication (also known as Client
    /// Certificate Authentication) within the Verifalia API.
    /// </summary>
    public sealed class ClientCertificate
    {
        /// <summary>
        /// A server-generated unique identifier for the certificate.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the external entity (user) to whom the certificate is issued.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }
        
        /// <summary>
        /// The certificate authority (CA) that issued and signed the certificate.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// A hash of the certificate, used to make comparisons easier.
        /// </summary>
        [JsonProperty("thumbprint")]
        public string Thumbprint { get; set; }
        
        /// <summary>
        /// Represents the public key contained in the certificate, used to verify the identity of the certificate holder
        /// and encrypt data that only the matching private key can decrypt.
        /// </summary>
        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }
        
        /// <summary>
        /// The timestamp when the certificate was created in Verifalia.
        /// </summary>
        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }
        
        /// <summary>
        /// The timestamp indicating when the certificate becomes valid.
        /// </summary>
        [JsonProperty("notBefore")]
        public DateTime NotBefore { get; set; }
        
        /// <summary>
        /// The timestamp indicating when the certificate expires.
        /// </summary>
        [JsonProperty("notAfter")]
        public DateTime NotAfter { get; set; }
    }
}
/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2019 Cobisi Research
*
* Cobisi Research
* Via Prima Strada, 35
* 35129, Padova
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

#if HAS_CLIENT_CERTIFICATES_SUPPORT

using System.Threading;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Verifalia.Api.Security
{
    internal class ClientCertificateAuthenticationProvider : IAuthenticationProvider
    {
        private readonly X509Certificate2 _certificate;

        internal class X509HttpFactory : DefaultHttpClientFactory
        {
            private readonly X509Certificate2 _certificate;

            public X509HttpFactory(X509Certificate2 cert)
            {
                _certificate = cert;
            }

            public override HttpMessageHandler CreateMessageHandler()
            {
                var handler = (HttpClientHandler)base.CreateMessageHandler();

                handler.ClientCertificates.Clear();
                handler.ClientCertificates.Add(_certificate);

                return handler;
            }
        }

        public ClientCertificateAuthenticationProvider(X509Certificate2 certificate)
        {
            if (certificate == null) throw new ArgumentNullException(nameof(certificate));

            if (!certificate.HasPrivateKey)
            {
                throw new ArgumentException("The given certificate does not have a private key and can't thus be used for authentication purposes.", nameof(certificate));
            }

            _certificate = certificate;
        }

        public Task ProvideAuthenticationAsync(IRestClient restClient, CancellationToken cancellationToken = default)
        {
            if (restClient == null) throw new ArgumentNullException(nameof(restClient));

            restClient
                .UnderlyingClient
                .Configure(settings => settings.HttpClientFactory = new X509HttpFactory(_certificate));

            return Task.CompletedTask;
        }
    }
}

#endif
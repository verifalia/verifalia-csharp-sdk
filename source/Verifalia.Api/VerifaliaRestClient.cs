/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2021 Cobisi Research
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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Verifalia.Api.BaseUrisProviders;
using Verifalia.Api.Credits;
using Verifalia.Api.EmailValidations;
using Verifalia.Api.Security;

namespace Verifalia.Api
{
    /// <inheritdoc cref="IVerifaliaRestClient"/>
    public class VerifaliaRestClient : IRestClientFactory, IVerifaliaRestClient, IDisposable
    {
        /// <summary>
        /// The default API version used by this SDK.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public const string DefaultApiVersion = "v2.4";

        private readonly Random _uriShuffler;
        private readonly IAuthenticationProvider _authenticator;
        private readonly BaseUrisProvider _baseUrisProvider;

        private string _apiVersion;

        // ReSharper disable once MemberCanBePrivate.Global
        internal MultiplexedRestClient? CachedRestClient; // Marked as internal for unit testing purposes

        /// <inheritdoc cref="IVerifaliaRestClient.ApiVersion"/>
        public string ApiVersion
        {
            get => _apiVersion;
            set
            {
                _apiVersion = value ?? throw new ArgumentNullException(nameof(value));

                // Invalidate the cached REST client on changes

                CachedRestClient = null;
            }
        }

        /// <inheritdoc cref="IVerifaliaRestClient.EmailValidations"/>
        public IEmailValidationsRestClient EmailValidations
        {
            get;
        }

        /// <inheritdoc cref="IVerifaliaRestClient.Credits"/>
        public ICreditsRestClient Credits
        {
            get;
        }

        /// <summary>
        /// Initializes a new HTTPS-based REST client for Verifalia with the specified username and password.
        /// <remarks>While authenticating with your Verifalia main account credentials is possible, it is strongly advised
        /// to create one or more users (formerly known as sub-accounts) with just the required permissions, for improved
        /// security. To create a new user or manage existing ones, please visit https://verifalia.com/client-area#/users </remarks>
        /// </summary>
        public VerifaliaRestClient(string username, string password, Uri[]? baseUris = default)
            : this(new UsernamePasswordAuthenticationProvider(username, password), baseUris)
        {
        }

#if HAS_CLIENT_CERTIFICATES_SUPPORT

        /// <summary>
        /// Initializes a new HTTPS-based REST client for Verifalia with the specified client certificate (for enterprise-grade
        /// mutual TLS authentication).
        /// <remarks>TLS client certificate authentication is available to premium plans only.</remarks>
        /// <remarks>It is strongly advised to create one or more users (formerly known as sub-accounts) with just the required
        /// permissions,
        /// for improved security. To create a new user or manage existing ones, please visit https://verifalia.com/client-area#/users
        /// </remarks>
        /// </summary>
        public VerifaliaRestClient(X509Certificate2 clientCertificate, Uri[]? baseUris = default)
            : this(new ClientCertificateAuthenticationProvider(clientCertificate), baseUris == default(Uri[]) ? new ClientCertificateBaseUrisProvider() : new BaseUrisProvider(baseUris))
        {
        }

#endif

        /// <summary>
        /// Initializes a new HTTPS-based REST client for Verifalia with the specified authentication provider.
        /// <remarks>While authenticating with your Verifalia main account credentials is possible, it is strongly advised
        /// to create one or more users (formerly known as sub-accounts) with just the required permissions, for improved
        /// security. To create a new user or manage existing ones, please visit https://verifalia.com/client-area#/users </remarks>
        /// </summary>
        public VerifaliaRestClient(IAuthenticationProvider authenticationProvider, Uri[]? baseUris = default)
            : this(authenticationProvider, baseUris == default(Uri[]) ? new DefaultBaseUrisProvider() : new BaseUrisProvider(baseUris))
        {
        }

        private VerifaliaRestClient(IAuthenticationProvider authenticator, BaseUrisProvider baseUrisProvider)
        {
            _authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));
            _baseUrisProvider = baseUrisProvider ?? throw new ArgumentNullException(nameof(baseUrisProvider));

            // Initialize the shuffle mechanism which randomizes the API endpoints usage

            _uriShuffler = new Random();
            _apiVersion = DefaultApiVersion;

            EmailValidations = new EmailValidationsRestClient(this);
            Credits = new CreditsRestClient(this);
        }

        /// <summary>
        /// Builds a REST client which peeks a random Verifalia API endpoint and automatically retries on the others, in the event
        /// of a network (or server error) failure.
        /// </summary>
        IRestClient IRestClientFactory.Build()
        {
            if (CachedRestClient != null)
                return CachedRestClient;

            // The user agent string brings the type of the client and its version (for statistical purposes
            // at the server side):

            var userAgent = String.Format(CultureInfo.InvariantCulture,
                "verifalia-rest-client/{0}/{1}",
#if NET45
                "net45",
#elif NET451
                "net451",
#elif NET452
                "net452",
#elif NET46
                "net46",
#elif NET461
                "net461",
#elif NET462
                "net462",
#elif NET47
                "net47",
#elif NET471
                "net471",
#elif NET472
                "net472",
#elif NET48
                "net48",
#elif NETSTANDARD1_3
                "netstandard1.3",
#elif NETSTANDARD1_4
                "netstandard1.4",
#elif NETSTANDARD1_5
                "netstandard1.5",
#elif NETSTANDARD1_6
                "netstandard1.6",
#elif NETSTANDARD1_7
                "netstandard1.7",
#elif NETSTANDARD2_0
                "netstandard2.0",
#elif NETSTANDARD2_1
                "netstandard2.1",
#elif NETCOREAPP1_0
                "netcoreapp1.0",
#elif NETCOREAPP1_1
                "netcoreapp1.1",
#elif NETCOREAPP2_0
                "netcoreapp2.0",
#elif NETCOREAPP2_1
                "netcoreapp2.1",
#elif NETCOREAPP2_2
                "netcoreapp2.2",
#elif NETCOREAPP3_0
                "netcoreapp3.0",
#elif NETCOREAPP3_1
                "netcoreapp3.1",
// Note: starting .NET 5.0, TFM compilation constants mean "version X or greater", see: https://github.com/dotnet/sdk/issues/13377
#elif NET7_0
                "net7.0",
#elif NET6_0
                "net6.0",
#elif NET5_0
                "net5.0",
#else
#error Unsupported platform.
#endif
                typeof(VerifaliaRestClient).GetTypeInfo().Assembly.GetName().Version);

            // Setup the REST client

            var shuffledFinalUris = _baseUrisProvider
                .ProvideBaseUris()
                .Select(uri => new Uri(uri, ApiVersion))
                .OrderBy(uri => _uriShuffler.Next());

            CachedRestClient = new MultiplexedRestClient(_authenticator,
                userAgent,
                shuffledFinalUris);

            return CachedRestClient;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                CachedRestClient?.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
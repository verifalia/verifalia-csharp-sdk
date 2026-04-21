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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Verifalia.Api.BaseUrisProviders;
using Verifalia.Api.ClientCertificates;
using Verifalia.Api.ContactMethods;
using Verifalia.Api.Credits;
using Verifalia.Api.EmailVerifications;
using Verifalia.Api.Security;
using Verifalia.Api.Users;

namespace Verifalia.Api
{
    /// <summary>
    /// Represents the main entry point of the Verifalia SDK for .NET, a library that enables interaction with the
    /// Verifalia API quickly and easily. Verifalia is a fast and accurate email verification service, which offers several
    /// unique integration options and advanced configuration: this class exposes all the features of the SDK through a fluent
    /// model, where each property exposes a set of methods that refer to a specific feature.
    /// <ul>
    /// <li><see cref="ClientCertificates"/> enables management of X.509 client certificates</li>
    /// <li><see cref="ContactMethods"/> enables management of contact methods</li>
    /// <li><see cref="Credits"/> enables management of credits and their usage consumption</li>
    /// <li><see cref="EmailVerifications"/> enables email address verification and management of email verification jobs</li>
    /// <li><see cref="Users"/> enables management of users and browser apps, along with their security and configuration settings</li>
    /// </ul>
    /// <example>
    /// For instance, to verify an email address or run an email verification, you would call one of the
    /// <see cref="IEmailVerificationsClient.RunAsync(string,Verifalia.Api.EmailVerifications.Models.QualityLevelName?,Verifalia.Api.EmailVerifications.WaitOptions?,System.Threading.CancellationToken)"/> method overloads of the <see cref="EmailVerifications"/> property.
    /// <code>
    /// var verifalia = new VerifaliaClient(/* ... */);
    /// var verification = await verifalia.EmailVerifications.RunAsync("batman@gmail.com");
    /// </code>
    /// </example>
    /// <example>
    /// Similarly, to retrieve the credit balance of your account, you would call the <see cref="ICreditsClient.GetBalanceAsync"/>
    /// method of the <see cref="Credits"/> property:
    /// <code>
    /// var verifalia = new VerifaliaClient(/* ... */);
    /// var balance = await verifalia.Credits.GetBalanceAsync();
    /// </code>
    /// </example>
    /// </summary>
    /// <inheritdoc cref="IVerifaliaClient"/>    
    public class VerifaliaClient : IRestClientFactory, IVerifaliaClient, IDisposable
    {
        /// <summary>
        /// The default API version used by this SDK.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public const string DefaultApiVersion = "v2.7";

        private readonly Random _uriShuffler;
        private readonly IAuthenticationProvider _authenticator;
        private readonly BaseUrisProvider _baseUrisProvider;

        private string _apiVersion;

        // ReSharper disable once MemberCanBePrivate.Global
        internal MultiplexedRestClient? CachedRestClient; // Marked as internal for unit testing purposes

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

        public IClientCertificatesClient ClientCertificates
        {
            get;
        }
        
        public IContactMethodsClient ContactMethods
        {
            get;
        }
        
        public ICreditsClient Credits
        {
            get;
        }

        public IEmailVerificationsClient EmailVerifications
        {
            get;
        }
        
        public IUsersClient Users
        {
            get;
        }
        
        /// <summary>
        /// Initializes a new <see cref="VerifaliaClient"/> with the specified username and password.
        /// </summary>
        /// <remarks>While authenticating with your Verifalia main account credentials is possible, it is strongly recommended
        /// to create one or more users with just the required permissions, for improved
        /// security. To create a new user or manage existing ones, please visit https://app.verifalia.com/#/users</remarks>
        public VerifaliaClient(string username, string password, Uri[]? baseUris = null)
            : this(new UsernamePasswordAuthenticationProvider(username, password), baseUris)
        {
        }

#if HAS_CLIENT_CERTIFICATES_SUPPORT
        /// <summary>
        /// Initializes a new <see cref="VerifaliaClient"/> with the specified client certificate (for enterprise-grade
        /// mutual TLS authentication).
        /// </summary>
        /// <remarks>
        /// It is strongly recommended to create one or more users (formerly known as sub-accounts) with just the required
        /// permissions for improved security. To create a new user or manage existing ones, please visit https://app.verifalia.com/#/users
        /// </remarks>
        /// <remarks>
        /// TLS client certificate authentication is available to premium plans only.
        /// To upgrade your plan please visit https://app.verifalia.com/#/account/change-plan
        /// </remarks>
        public VerifaliaClient(X509Certificate2 clientCertificate, Uri[]? baseUris = null)
            : this(new ClientCertificateAuthenticationProvider(clientCertificate), baseUris == null ? new ClientCertificateBaseUrisProvider() : new BaseUrisProvider(baseUris))
        {
        }
#endif

        /// <summary>
        /// Initializes a new <see cref="VerifaliaClient"/> with the specified authentication provider.
        /// </summary>
        /// <remarks>While authenticating with your Verifalia main account credentials is possible, it is strongly recommended
        /// to create one or more users (formerly known as sub-accounts) with just the required permissions, for improved
        /// security. To create a new user or manage existing ones, please visit https://app.verifalia.com/#/users</remarks>
        public VerifaliaClient(IAuthenticationProvider authenticationProvider, Uri[]? baseUris = null)
            : this(authenticationProvider, baseUris == null ? new DefaultBaseUrisProvider() : new BaseUrisProvider(baseUris))
        {
        }

        private VerifaliaClient(IAuthenticationProvider authenticator, BaseUrisProvider baseUrisProvider)
        {
            _authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));
            _baseUrisProvider = baseUrisProvider ?? throw new ArgumentNullException(nameof(baseUrisProvider));

            // Initialize the shuffle mechanism which randomizes the API endpoints usage

            _uriShuffler = new Random();
            _apiVersion = DefaultApiVersion;

            ClientCertificates = new ClientCertificatesClient(this);
            ContactMethods = new ContactMethodsClient(this);
            Credits = new CreditsClient(this);
            EmailVerifications = new EmailVerificationsClient(this);
            Users = new UsersClient(this);
        }

        /// <summary>
        /// Creates an <see cref="IRestClient"/> instance that selects a random Verifalia API endpoint and automatically retries on other endpoints
        /// in the event of a network or server error failure.
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
#elif NET10_0
                "net10.0",
#elif NET9_0
                "net9.0",
#elif NET8_0
                "net8.0",
#elif NET7_0
                "net7.0",
#elif NET6_0
                "net6.0",
#elif NET5_0
                "net5.0",
#else
#error Unsupported platform.
#endif
                typeof(VerifaliaClient).GetTypeInfo().Assembly.GetName().Version);

            // Setup the REST client

            var shuffledFinalUris = _baseUrisProvider
                .ProvideBaseUris()
                .Select(uri => new Uri(uri, ApiVersion))
                .OrderBy(_ => _uriShuffler.Next());

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
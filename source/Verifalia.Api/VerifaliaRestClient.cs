using System;
using System.Linq;
using System.Reflection;
using Verifalia.Api.AccountBalance;
using Verifalia.Api.EmailAddresses;

namespace Verifalia.Api
{
    /// <summary>
    /// REST client for Verifalia API.
    /// </summary>
    public class VerifaliaRestClient : IRestClientFactory, IVerifaliaRestClient
    {
        private const string DefaultApiVersion = "v1.4";

        private readonly Random _uriShuffler;
        private readonly string _accountSid;
        private readonly string _authToken;

        private Uri[] _baseUris;
        private string _apiVersion;
        private MultiplexedRestClient _cachedRestClient;

        private IValidationRestClient _emailValidations;
        private IAccountBalanceRestClient _accountBalance;

        public static Uri[] DefaultBaseUris { get; }

        /// <summary>
        /// Verifalia API version to use when making requests.
        /// </summary>
        public string ApiVersion
        {
            get => _apiVersion;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                _apiVersion = value;
            }
        }

        /// <summary>
        /// Base URIs of Verifalia API (defaults to https://api-1.verifalia.com/ and https://api-2.verifalia.com). Do not use this property
        /// unless instructed to do so by the Verifalia support team.
        /// </summary>
        public Uri[] BaseUris
        {
            get => _baseUris;
            set
            {
                if (value == null)
                {
                    _baseUris = DefaultBaseUris;
                }
                else
                {
                    // Custom base URIs

                    if (value.Length == 0)
                    {
                        throw new ArgumentException("Value must contain at least one base URI.", nameof(value));
                    }

                    _baseUris = value;
                }

                // Invalidate the cached rest client

                _cachedRestClient = null;
            }
        }

        /// <summary>
        /// Allows to submit and manage email validations using the Verifalia service.
        /// </summary>
        public IValidationRestClient EmailValidations => _emailValidations ?? (_emailValidations = new ValidationRestClient(this));

        /// <summary>
        /// Allows to manage the credits for the Verifalia account.
        /// </summary>
        public IAccountBalanceRestClient AccountBalance => _accountBalance ?? (_accountBalance = new AccountBalanceRestClient(this));

        static VerifaliaRestClient()
        {
            // Default base URIs

            DefaultBaseUris = new[]
            {
                new Uri("https://api-1.verifalia.com"),
                new Uri("https://api-2.verifalia.com")
            };
        }

        /// <summary>
        /// Initializes a new Verifalia REST client with the specified sub-account credentials. Sub-accounts
        /// for API usage, along with their Account SID and Auth Token, can be set up from within the Verifalia dashboard, at 
        /// <seealso cref="https://verifalia.com/client-area">https://verifalia.com/client-area</seealso>, in the "API and
        /// sub-accounts" section.
        /// </summary>
        public VerifaliaRestClient(string accountSid, string authToken)
        {
            if (String.IsNullOrEmpty(accountSid))
                throw new ArgumentException("accountSid is null or empty: please visit https://verifalia.com/client-area to set up a new API sub-account if you don't have one.", nameof(accountSid));
            if (String.IsNullOrEmpty(authToken))
                throw new ArgumentException("authToken is null or empty: please visit https://verifalia.com/client-area to set up a new API sub-account if you don't have one.", nameof(authToken));

            _accountSid = accountSid;
            _authToken = authToken;

            // Initialize the shuffle mechanism which randomizes the API endpoints usage

            _uriShuffler = new Random();

            // Default values used to build the base URL needed to access the service

            _baseUris = DefaultBaseUris;
            _apiVersion = DefaultApiVersion;
        }

        /// <summary>
        /// Builds a custom REST client which peeks a random API endpoint and automatically retries on the others, in the event
        /// of a network failure.
        /// </summary>
        IRestClient IRestClientFactory.Build()
        {
            if (_cachedRestClient != null)
                return _cachedRestClient;

            // The user agent string brings the type of the client and its version (for statistical purposes
            // at the server side):

            var userAgent = String.Format("verifalia-rest-client/{0}/{1}",
#if NET45
                "net45",
#elif NETSTANDARD1_4
                "netstandard1.4",
#else
#error Unsupported platform.
#endif
                typeof(VerifaliaRestClient).GetTypeInfo().Assembly.GetName().Version);

            // Setup the REST client

            var shuffledFinalUris = _baseUris
                .Select(uri => new Uri(uri, ApiVersion))
                .OrderBy(uri => _uriShuffler.Next());

            _cachedRestClient = new MultiplexedRestClient(shuffledFinalUris,
                _accountSid,
                _authToken,
                userAgent);

            return _cachedRestClient;
        }
    }
}
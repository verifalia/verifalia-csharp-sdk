using System;
using System.Linq;
using System.Reflection;
using RestSharp;
using RestSharp.Authenticators;
using Verifalia.Api.EmailAddresses;

namespace Verifalia.Api
{
    /// <summary>
    /// REST client for Verifalia API.
    /// </summary>
    public class VerifaliaRestClient : IRestClientFactory
    {
        const string DefaultApiVersion = "v1.2";

        readonly Random _uriShuffler;
        readonly string _accountSid;
        readonly string _authToken;

        Uri[] _baseUris;
        string _apiVersion;

        ValidationRestClient _emailValidations;

        /// <summary>
        /// Verifalia API version to use when making requests.
        /// </summary>
        public string ApiVersion
        {
            get
            {
                return _apiVersion;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _apiVersion = value;
            }
        }

        /// <summary>
        /// Base URIs of Verifalia API (defaults to https://api-1.verifalia.com/ and https://api-2.verifalia.com). Do not use this property
        /// unless instructed to so by the Verifalia support team.
        /// </summary>
        public Uri[] BaseUris
        {
            get
            {
                return _baseUris;
            }
            set
            {
                if (value == null)
                {
                    SetDefaultBaseUris();
                }
                else
                {
                    // Custom base URIs

                    if (value.Length == 0)
                    {
                        throw new ArgumentException("Value must contain at least one base URI.", "value");
                    }

                    _baseUris = value;
                }
            }
        }

        /// <summary>
        /// Allows to submit and manage email validations using the Verifalia service.
        /// </summary>
        public ValidationRestClient EmailValidations
        {
            get
            {
                return _emailValidations ?? (_emailValidations = new ValidationRestClient(this));
            }
        }

        /// <summary>
        /// Initializes a new Verifalia REST client with the specified sub-account credentials. Sub-accounts
        /// for API usage, along with their Account SID and Auth Token, can be set up from within the Verifalia dashboard, at 
        /// <seealso cref="https://verifalia.com/client-area">https://verifalia.com/client-area</seealso>.
        /// </summary>
        public VerifaliaRestClient(string accountSid, string authToken)
        {
            if (String.IsNullOrEmpty(accountSid))
                throw new ArgumentException("accountSid is null or empty.", "accountSid");
            if (String.IsNullOrEmpty(authToken))
                throw new ArgumentException("authToken is null or empty.", "authToken");

            _accountSid = accountSid;
            _authToken = authToken;

            _uriShuffler = new Random();

            // Default values used to build the base URL needed to access the service

            SetDefaultBaseUris();
            _apiVersion = DefaultApiVersion;
        }

        void SetDefaultBaseUris()
        {
            // Default base URIs

            _baseUris = new[]
            {
                new Uri("https://api-1.verifalia.com"),
                new Uri("https://api-2.verifalia.com")
            };
        }

        /// <summary>
        /// Builds a custom REST client which peek a random API endpoint and automatically retries on the others, in the event of a network failure.
        /// </summary>
        RestClient IRestClientFactory.Build()
        {
            // The user agent string brings the type of the client and its version (for statistical purposes
            // at the server side):

            var userAgent = String.Concat("verifalia-rest-client/netfx/",
                Assembly.GetExecutingAssembly().GetName().Version);

            // Setup the REST client

            var shuffledFinalUris = _baseUris
                .Select(uri => new Uri(uri, ApiVersion))
                .OrderBy(uri => _uriShuffler.Next());

            var restClient = new MultiplexedRestClient(shuffledFinalUris)
            {
                Authenticator = new HttpBasicAuthenticator(_accountSid, _authToken),
                UserAgent = userAgent
            };

            restClient.ClearHandlers();
            restClient.AddHandler("application/json", new ProgressiveJsonSerializer());
            restClient.FollowRedirects = false;

            return restClient;
        }
    }
}
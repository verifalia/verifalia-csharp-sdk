using System;
using System.Reflection;
using RestSharp;
using Verifalia.Api.EmailAddresses;

namespace Verifalia.Api
{
    /// <summary>
    /// REST client for Verifalia API.
    /// </summary>
    public class VerifaliaRestClient
    {
        const string DefaultApiVersion = "v1";
        const string DefaultBaseUrl = "https://api.verifalia.com/";

        readonly RestClient _restClient;
        string _apiVersion;
        Uri _baseUri;

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
                UpdateRestClientBaseUrl();
            }
        }

        /// <summary>
        /// Base URL of Verifalia API (defaults to https://api.verifalia.com/)
        /// </summary>
        public Uri BaseUri
        {
            get
            {
                return _baseUri;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _baseUri = value;
                UpdateRestClientBaseUrl();
            }
        }

        /// <summary>
        /// Allows to submit and manage email validations using the Verifalia service.
        /// </summary>
        public ValidationRestClient EmailValidations
        {
            get
            {
                return _emailValidations ?? (_emailValidations = new ValidationRestClient(_restClient));
            }
        }

        /// <summary>
        /// Initializes a new Verifalia REST client with the specified credentials.
        /// </summary>
        /// <remarks>Your Account SID and Auth token values can be retrieved in your client area,
        /// upon clicking on your subscription details, on Verifalia website at: https://verifalia.com/client-area/subscriptions </remarks>
        public VerifaliaRestClient(string accountSid, string authToken)
        {
            if (String.IsNullOrEmpty(accountSid))
                throw new ArgumentException("accountSid is null or empty.", "accountSid");
            if (String.IsNullOrEmpty(authToken))
                throw new ArgumentException("authToken is null or empty.", "authToken");

            // Default values used to build the base URL needed to access the service

            _apiVersion = DefaultApiVersion;
            _baseUri = new Uri(DefaultBaseUrl);

            // The user agent string brings the type of the client and its version (for statistical purposes
            // at the server side):

            var userAgent = String.Concat("verifalia-rest-client/netfx/",
                Assembly.GetExecutingAssembly().GetName().Version);

            // Setup the rest client

            _restClient = new RestClient
            {
                Authenticator = new HttpBasicAuthenticator(accountSid, authToken),
                UserAgent = userAgent
            };

            UpdateRestClientBaseUrl();
        }

        /// <summary>
        /// Updates the base URL for the underlying REST client.
        /// </summary>
        void UpdateRestClientBaseUrl()
        {
            _restClient.BaseUrl = new Uri(BaseUri, ApiVersion).AbsoluteUri;
        }
    }
}
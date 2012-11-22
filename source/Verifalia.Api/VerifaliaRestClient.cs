using System;
using System.Reflection;
using RestSharp;

namespace Verifalia.Api
{
    /// <summary>
    /// REST client for Verifalia API.
    /// </summary>
    public partial class VerifaliaRestClient
    {
        const string DEFAULT_API_VERSION = "v1";
        const string DEFAULT_BASE_URL = "https://api.verifalia.com/";

        readonly string _AccountSid;
        readonly RestClient _RestClient;

        string _ApiVersion;
        Uri _BaseUri;

        /// <summary>
        /// Verifalia API version to use when making requests.
        /// </summary>
        public string ApiVersion
        {
            get
            {
                return _ApiVersion;
            }
            set
            {
                _ApiVersion = value;
                UpdateBaseUri();
            }
        }

        /// <summary>
        /// Base URL of Verifalia API (defaults to https://api.verifalia.com/)
        /// </summary>
        public Uri BaseUri
        {
            get
            {
                return _BaseUri;
            }
            set
            {
                _BaseUri = value;
                UpdateBaseUri();
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

            _AccountSid = accountSid;

            // Default values used to build the base URL needed to access the service

            _ApiVersion = DEFAULT_API_VERSION;
            _BaseUri = new Uri(DEFAULT_BASE_URL);

            // Setup the rest client

            _RestClient = new RestClient();
            _RestClient.Authenticator = new HttpBasicAuthenticator(_AccountSid, authToken);

            UpdateBaseUri();

            // The user agent string brings the type of the client and its version (for statistical purposes
            // at the server side):

            var executingAssemblyVersion = Assembly.GetExecutingAssembly()
                .GetName()
                .Version;

            _RestClient.UserAgent = String.Concat("verifalia-rest-client/netfx/", executingAssemblyVersion);
        }

        /// <summary>
        /// Updates the base URL for the underlying REST client.
        /// </summary>
        void UpdateBaseUri()
        {
            _RestClient.BaseUrl = new Uri(BaseUri, ApiVersion).AbsoluteUri;
        }
    }
}

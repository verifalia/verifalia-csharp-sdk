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

#if HAS_JWT_SUPPORT

using System.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using Verifalia.Api.Exceptions;

namespace Verifalia.Api.Security
{
    /// <summary>
    /// Allows to authenticate a REST client against the Verifalia API using bearer authentication.
    /// </summary>
    public class BearerAuthenticationProvider : IAuthenticationProvider
    {
        internal class BearerAuthenticationResponseModel
        {
            [JsonProperty("accessToken")] public string AccessToken { get; set; }
        }

        private const string JwtClaimMfaRequiredName = "verifalia:mfa";
        private const string AuthorizationHttpHeaderName = "Authorization"; 
        private const int MaxNoOfMfaAttempts = 5;

        private readonly string _username;
        private readonly string _password;
        private readonly ITotpTokenProvider? _totpTokenProvider;

        private JwtSecurityToken? _securityToken;

        /// <summary>
        /// Initializes a new bearer authentication provider for the Verifalia API, with the specified username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="totpTokenProvider">An optional provider of TOTP tokens (needed if the user has multi-factor authentication enabled).</param>
        public BearerAuthenticationProvider(string username, string password, ITotpTokenProvider? totpTokenProvider = default)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username),
                    "username is null or empty: please visit https://verifalia.com/client-area to set up a new user, if you don't have one.");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password),
                    "password is null or empty: please visit https://verifalia.com/client-area to set up a new user, if you don't have one.");
            }

            _username = username;
            _password = password;
            _totpTokenProvider = totpTokenProvider;
        }

        public async Task AuthenticateAsync(IRestClient restClient, CancellationToken cancellationToken = default)
        {
            if (restClient == null) throw new ArgumentNullException(nameof(restClient));

            // Request a new security token to the Verifalia API, if one is not yet available

            if (_securityToken == null)
            {
                // Remove any eventual authorization header and request the token

                restClient.UnderlyingClient.WithHeader(AuthorizationHttpHeaderName, null);

                var content = restClient.Serialize(new
                {
                    username = _username,
                    password = _password
                });

                using var authResponse = await restClient.InvokeAsync(HttpMethod.Post,
                        "/auth/tokens",
                        contentFactory: _ => Task.FromResult<HttpContent>(new StringContent(content, Encoding.UTF8, WellKnownMimeContentTypes.ApplicationJson)),
                        // Avoid using the configured authentication provider - as auth tokens must be retrieved using HTTP basic auth
                        skipAuthentication: true,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                
                if (authResponse.StatusCode == HttpStatusCode.OK)
                {
                    var bearerAuthenticationResponse = await authResponse
                        .Content
                        .DeserializeAsync<BearerAuthenticationResponseModel>(restClient)
                        .ConfigureAwait(false);

                    // Handle the multi-factor auth (MFA) request, if needed

                    _securityToken = (JwtSecurityToken) new JwtSecurityTokenHandler()
                        .ReadToken(bearerAuthenticationResponse.AccessToken);

                    var mfaRequiredClaim = _securityToken
                        .Claims
                        .FirstOrDefault(claim => claim.Type == JwtClaimMfaRequiredName);

                    if (mfaRequiredClaim != null)
                    {
                        // Requests a new bearer token, passing the TOTP first

                        bearerAuthenticationResponse = await ProvideAdditionalAuthFactorAsync(restClient, cancellationToken)
                            .ConfigureAwait(false);

                        _securityToken = (JwtSecurityToken) new JwtSecurityTokenHandler()
                            .ReadToken(bearerAuthenticationResponse.AccessToken);
                    }
                }
                else
                {
                    throw new AuthorizationException(
                        "Invalid credentials used while attempting to retrieve a bearer auth token.");
                }
            }

            AddBearerAuth(restClient);
        }

        /// <inheritdoc cref="IAuthenticationProvider.AuthenticateAsync(IRestClient, CancellationToken)"/>
        private async Task<BearerAuthenticationResponseModel> ProvideAdditionalAuthFactorAsync(IRestClient restClient, CancellationToken cancellationToken)
        {
            if (restClient == null) throw new ArgumentNullException(nameof(restClient));

            if (_totpTokenProvider == null)
            {
                throw new AuthorizationException(
                    "A multi-factor authentication is required but no token provider has been provided.");
            }

            for (var idxAttempt = 0; idxAttempt < MaxNoOfMfaAttempts; idxAttempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                // Retrieve the one-time token from the configured device

                var totp = await _totpTokenProvider
                    .ProvideTotpTokenAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                // Validates the provided token against the Verifalia API

                try
                {
                    AddBearerAuth(restClient);

                    using var authResponse = await restClient
                        .InvokeAsync(HttpMethod.Post,
                            "/auth/totp/verifications",
                            contentFactory: _ => Task.FromResult<HttpContent>(new StringContent(restClient.Serialize(new
                            {
                                passCode = totp
                            }), Encoding.UTF8, WellKnownMimeContentTypes.ApplicationJson)),
                            // Avoid using the configured authentication provider - as auth tokens must be retrieved using HTTP basic auth
                            skipAuthentication: true,
                            cancellationToken: cancellationToken)
                        .ConfigureAwait(false);

                    if (authResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return await authResponse
                            .Content
                            .DeserializeAsync<BearerAuthenticationResponseModel>(restClient)
                            .ConfigureAwait(false);
                    }
                }
                catch (AuthorizationException)
                {
                    // Having an authorization issue is allowed here, as we are working on an TOTP token validation attempt.
                    // We will re-throw an AuthorizationException below in the even all the configured TOTP validation attempts fail. 
                }
            }
            
            throw new AuthorizationException($"Invalid TOTP token provided after {MaxNoOfMfaAttempts} attempt(s): aborting the authentication.");
        }

        /// <inheritdoc cref="IAuthenticationProvider.HandleUnauthorizedRequestAsync(IRestClient, CancellationToken)"/>
        public Task HandleUnauthorizedRequestAsync(IRestClient restClient, CancellationToken cancellationToken)
        {
            if (restClient == null) throw new ArgumentNullException(nameof(restClient));
            
            // Invalidates the stored security token, which will be acquired again in the next AuthenticateAsync() invocation
            
            _securityToken = null;
            
            // TODO: We may want to refresh the token, instead of forcing the library to re-acquire a new one

#if HAS_TASK_COMPLETED_TASK
            return Task.CompletedTask;
#else
            return Task.FromResult((object) null);
#endif
        }

        private void AddBearerAuth(IRestClient restClient)
        {
            restClient.UnderlyingClient.WithHeader(AuthorizationHttpHeaderName, $"Bearer {_securityToken.RawData}");
        }
    }
}

#endif
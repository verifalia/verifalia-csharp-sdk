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

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using Verifalia.Api.Exceptions;

namespace Verifalia.Api.Security
{
    internal class BearerAuthenticationProvider : IAuthenticationProvider
    {
        internal class BearerAuthenticationResponseModel
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }
        }

        private readonly string _username;
        private readonly string _password;
        
        private string _cachedAccessToken;

        public BearerAuthenticationProvider(string username, string password)
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
        }

        public async Task ProvideAuthenticationAsync(IRestClient restClient, CancellationToken cancellationToken = default)
        {
            // TODO: Use the cached access token, if available
            // TODO: How to check if the access token is expired?

            restClient.UnderlyingClient.WithBasicAuth(_username, _password);

            var content = restClient.Serialize(new
            {
                username = _username,
                password = _password
            });

            using var postedContent = new StringContent(content, Encoding.UTF8, WellKnownMimeContentTypes.ApplicationJson);
            using var authResponse = await restClient.InvokeAsync(HttpMethod.Post,
                    "/auth/tokens",
                    content: postedContent,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var bearerAuthenticationResponse = await authResponse
                    .Content
                    .DeserializeAsync<BearerAuthenticationResponseModel>(restClient)
                    .ConfigureAwait(false);

                _cachedAccessToken = bearerAuthenticationResponse.AccessToken;
                restClient.UnderlyingClient.WithHeader("Authorization", $"Bearer {_cachedAccessToken}");
            }
            else
            {
                throw new AuthorizationException("Invalid credentials used while attempting to retrieve a bearer auth token.");
            }
        }
    }
}
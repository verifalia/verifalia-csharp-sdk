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
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace Verifalia.Api.Security
{
    /// <summary>
    /// Enables authentication of a REST client against the Verifalia API using a browser app key.
    /// </summary>
    /// <inheritdoc />
    public class AppkeyAuthenticatorProvider : IAuthenticationProvider
    {
        private readonly string _appKey;

        public AppkeyAuthenticatorProvider(string appKey)
        {
            if (String.IsNullOrEmpty(appKey))
            {
                throw new ArgumentNullException(nameof(appKey),
                    "appKey is null or empty: please visit https://app.verifalia.com/#/users to set up a new browser app, if you don't have one.");
            }

            _appKey = appKey;
        }

        /// <inheritdoc cref="IAuthenticationProvider.AuthenticateAsync(IRestClient, CancellationToken)"/>
        public Task AuthenticateAsync(IRestClient restClient, CancellationToken cancellationToken = default)
        {
            if (restClient == null) throw new ArgumentNullException(nameof(restClient));

            restClient.UnderlyingClient.WithBasicAuth(_appKey, String.Empty);

#if HAS_TASK_COMPLETED_TASK
            return Task.CompletedTask;
#else
            return Task.FromResult((object) null);
#endif
        }

        /// <inheritdoc cref="IAuthenticationProvider.HandleUnauthorizedRequestAsync(IRestClient, CancellationToken)"/>
        public Task<bool> HandleUnauthorizedRequestAsync(IRestClient restClient, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }
    }
}

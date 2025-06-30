/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2025 Cobisi Research
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

using System.Threading;
using System.Threading.Tasks;

namespace Verifalia.Api.Security
{
    /// <summary>
    /// Represents a type that can authenticate a REST client against the Verifalia API.
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// Authenticates the specified REST client.
        /// </summary>
        /// <param name="restClient">The REST client to authenticate.</param>
        /// <param name="cancellationToken">A token to cancel the authentication operation.</param>
        Task AuthenticateAsync(IRestClient restClient, CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to recover from an authentication failure by refreshing credentials or tokens.
        /// This method is useful for authentication providers that use time-limited credentials,
        /// such as JWT bearer tokens that may expire during operation.
        /// </summary>
        /// <param name="restClient">The REST client that encountered an unauthorized response.</param>
        /// <param name="cancellationToken">A token to cancel the recovery operation.</param>
        /// <returns>
        /// <c>true</c> if the authentication was successfully refreshed and the request should be retried;
        /// <c>false</c> if recovery failed and the unauthorized error should be returned to the caller.
        /// </returns>
        Task<bool> HandleUnauthorizedRequestAsync(IRestClient restClient, CancellationToken cancellationToken);
    }
}
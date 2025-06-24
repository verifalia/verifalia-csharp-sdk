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

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Users.Models;

namespace Verifalia.Api.Users
{
    /// <inheritdoc />
    internal partial class UsersClient
    {
        public async Task<User?> GetAsync(string userId, CancellationToken cancellationToken = default)
        {
            // Sends the request to the Verifalia servers

            var restClient = _restClientFactory.Build();

            using var response = await restClient
                .InvokeAsync(HttpMethod.Get,
                    $"users/{userId}",
                    headers: new Dictionary<string, object>
                    {
                        {
                            "Accept", WellKnownMimeContentTypes.ApplicationJson
                        }
                    },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    return await response
                        .Content
                        .DeserializeAsync<User>(restClient)
                        .ConfigureAwait(false);
                }

                case HttpStatusCode.NotFound:
                {
                    return null;
                }

                default:
                {
                    throw await restClient
                        .BuildRequestFailedExceptionAsync(response, cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }        
    }
}
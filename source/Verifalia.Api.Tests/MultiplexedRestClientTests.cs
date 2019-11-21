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
using System.Threading.Tasks;
using Flurl.Http.Testing;
using Verifalia.Api.Exceptions;
using Verifalia.Api.Security;
using Xunit;

namespace Verifalia.Api.Tests
{
    public class MultiplexedRestClientTests
    {
        private readonly IAuthenticator _authenticator = new UsernamePasswordAuthenticator("username", "password");

        [Fact]
        public async Task ShouldThrowServiceUnreachableExceptionWhenAllEndpointsReturnServerError()
        {
            using (var httpTest = new HttpTest())
            {
                // Two subsequent calls should result in a server error 5xx

                httpTest.RespondWith("Internal server error", 500);
                httpTest.RespondWith("Internal server error", 500);

                var client = new MultiplexedRestClient(_authenticator, "dummy", new[] { new Uri("https://dummy1"), new Uri("https://dummy2") });
                await Assert.ThrowsAsync<ServiceUnreachableException>(async () => await client.InvokeAsync(HttpMethod.Get, "dummy"));
            }
        }

        [Fact]
        public async Task ShouldThrowServiceUnreachableExceptionWhenAllEndpointsTimeOut()
        {
            using (var httpTest = new HttpTest())
            {
                // Two subsequent calls should result in a server error 5xx

                httpTest.SimulateTimeout();
                httpTest.SimulateTimeout();

                var client = new MultiplexedRestClient(_authenticator, "dummy", new[] { new Uri("https://dummy1"), new Uri("https://dummy2") });
                await Assert.ThrowsAsync<ServiceUnreachableException>(async () => await client.InvokeAsync(HttpMethod.Get, "dummy"));
            }
        }

        [Fact]
        public async Task ShouldSucceedWhenAtLeastOneEndpointDoesNotReturnServerError()
        {
            using (var httpTest = new HttpTest())
            {
                // Only the first call should result in a server error 5xx

                httpTest.RespondWith("Internal server error", 500);

                var client = new MultiplexedRestClient(_authenticator, "dummy", new[] { new Uri("https://dummy1"), new Uri("https://dummy2") });
                var response = await client.InvokeAsync(HttpMethod.Get, "dummy");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task ShouldSucceedWhenAtLeastOneEndpointDoesNotTimeOut()
        {
            using (var httpTest = new HttpTest())
            {
                // Only the first call should result in a server error 5xx

                httpTest.SimulateTimeout();

                var client = new MultiplexedRestClient(_authenticator, "dummy", new[] { new Uri("https://dummy1"), new Uri("https://dummy2") });
                var response = await client.InvokeAsync(HttpMethod.Get, "dummy");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }


        [Fact]
        public async Task ShouldImmediatelyFailOnForbiddenError()
        {
            using (var httpTest = new HttpTest())
            {
                // Only the first call should result in a 403 error

                httpTest.RespondWith("Forbidden", 403);

                var client = new MultiplexedRestClient(_authenticator, "dummy", new[] { new Uri("https://dummy1"), new Uri("https://dummy2") });
                await Assert.ThrowsAsync<AuthorizationException>(async () => await client.InvokeAsync(HttpMethod.Get, "dummy"));
            }
        }
    }
}

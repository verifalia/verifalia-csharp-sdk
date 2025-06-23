/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2024 Cobisi Research
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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.ClientCertificates.Models;
using Verifalia.Api.ContactMethods.Models;
using Verifalia.Api.Exceptions;

namespace Verifalia.Api.ClientCertificates
{
    /// <inheritdoc />
    internal sealed partial class ClientCertificatesClient
    {
        public async Task<ClientCertificate> CreateAsync(string userId, byte[] certificateFile, CancellationToken cancellationToken = default)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (certificateFile == null) throw new ArgumentNullException(nameof(certificateFile));
            
            using var stream = new MemoryStream(certificateFile);
            
            return await CreateAsync(userId,
                    stream,
                    cancellationToken)
                .ConfigureAwait(false);
        }
        
        public async Task<ClientCertificate> CreateAsync(string userId, Stream certificateFile, CancellationToken cancellationToken = default)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (certificateFile == null) throw new ArgumentNullException(nameof(certificateFile));

            // Sends the request to the Verifalia servers

            var restClient = _restClientFactory.Build();
            
            using var postedFileContent = new StreamContent(certificateFile);

            using var response = await restClient
                .InvokeAsync(HttpMethod.Post,
                    $"users/{userId}/certificates",
                    headers: new Dictionary<string, object>
                    {
                        {
                            "Accept", $"{WellKnownMimeContentTypes.ApplicationJson}, {WellKnownMimeContentTypes.ApplicationProblemJson}"
                        },
                    },
                    contentFactory: _ =>
                    {
                        var postedContent = new MultipartFormDataContent();

                        postedContent.Add(postedFileContent, "certificate",
                            // HACK: Must send a filename, as the backend expects one
                            // see https://github.com/dotnet/aspnetcore/blob/425c196cba530b161b120a57af8f1dd513b96f67/src/Http/Headers/src/ContentDispositionHeaderValueIdentityExtensions.cs#L27
                            "dummy");

                        return Task.FromResult<HttpContent>(postedContent);
                    },
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response
                    .Content
                    .DeserializeAsync<ClientCertificate>(restClient)
                    .ConfigureAwait(false);
            }
            
            // Unexpected error
            
            throw await restClient
                .BuildRequestFailedExceptionAsync(response, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
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

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.Common.JsonPatch;
using Verifalia.Api.ContactMethods.Models;

namespace Verifalia.Api.ContactMethods
{
    /// <inheritdoc />
    internal sealed partial class ContactMethodsClient
    {
        public async Task UpdateAsync(string userId, string contactMethodId, Expression<Func<ContactMethod, ContactMethod>> changeset, string? ifMatch = null, CancellationToken cancellationToken = default)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (contactMethodId == null) throw new ArgumentNullException(nameof(contactMethodId));
            if (changeset == null) throw new ArgumentNullException(nameof(changeset));
            
            // Sends the request to the Verifalia servers

            var restClient = _restClientFactory.Build();
            
            // Walk the expression and build up the JSON Patch document

            var jsonPatchDocument = new JsonPatchExpressionVisitor().BuildJsonPatchDocument(changeset);
            var content = restClient.Serialize(jsonPatchDocument);

            // Sends the request to the Verifalia servers

            var headers = new Dictionary<string, object>
            {
                {
                    "Accept", $"{WellKnownMimeContentTypes.ApplicationJson}, {WellKnownMimeContentTypes.ApplicationProblemJson}"
                }
            };
            
            // ETag match precondition

            if (ifMatch != null)
            {
                headers.Add("If-Match", ifMatch);
            }
            
            using var response = await restClient
                .InvokeAsync(new HttpMethod("PATCH"), 
                    $"users/{userId}/contact-methods/{contactMethodId}",
                    headers: headers,
                    contentFactory: _ => Task.FromResult<HttpContent>(new StringContent(content, Encoding.UTF8, WellKnownMimeContentTypes.ApplicationJsonPatch)),
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    return;
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
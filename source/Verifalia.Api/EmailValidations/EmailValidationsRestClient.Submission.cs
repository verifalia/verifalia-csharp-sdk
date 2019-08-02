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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.EmailValidations.Models;
using Verifalia.Api.Exceptions;

namespace Verifalia.Api.EmailValidations
{
    /// <inheritdoc />
    internal partial class EmailValidationsRestClient
    {
        public Task<Validation> SubmitAsync(string emailAddress, QualityLevelName quality = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));

            return SubmitAsync(new[] { emailAddress },
                quality: quality,
                waitingStrategy: waitingStrategy,
                cancellationToken: cancellationToken);
        }

        public Task<Validation> SubmitAsync(IEnumerable<string> emailAddresses, QualityLevelName quality = default, DeduplicationMode deduplication = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            if (emailAddresses == null) throw new ArgumentNullException(nameof(emailAddresses));

            return SubmitAsync(emailAddresses.Select(emailAddress => new ValidationRequestEntry(emailAddress)),
                quality: quality,
                deduplication: deduplication,
                waitingStrategy: waitingStrategy,
                cancellationToken: cancellationToken);
        }

        public Task<Validation> SubmitAsync(ValidationRequestEntry entry, QualityLevelName quality = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            return SubmitAsync(new[] { entry },
                quality: quality,
                waitingStrategy: waitingStrategy,
                cancellationToken: cancellationToken);
        }

        public Task<Validation> SubmitAsync(IEnumerable<ValidationRequestEntry> entries, QualityLevelName quality = default, DeduplicationMode deduplication = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            return SubmitAsync(new ValidationRequest(entries, quality, deduplication),
                waitingStrategy: waitingStrategy,
                cancellationToken: cancellationToken);
        }

        public async Task<Validation> SubmitAsync(ValidationRequest validationRequest, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            if (validationRequest == null) throw new ArgumentNullException(nameof(validationRequest));

            var restClient = _restClientFactory.Build();

            // Serialize the validation request to JSON

            var content = restClient
                .Serialize(new
                {
                    entries = validationRequest.Entries,
                    quality = validationRequest.Quality?.NameOrGuid,
                    deduplication = validationRequest.Deduplication?.NameOrGuid,
                    priority = validationRequest.Priority?.Value,
                    name = validationRequest.Name
                });

            // Send the request to the Verifalia servers

            using (var postedContent = new StringContent(content, Encoding.UTF8, WellKnowMimeContentTypes.ApplicationJson))
            using (var response = await restClient.InvokeAsync(HttpMethod.Post,
                    "email-validations",
                    queryParams: null,
                    headers: new Dictionary<string, object> { { "Accept", WellKnowMimeContentTypes.ApplicationJson } },
                    content: postedContent,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false))
            {

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                        {
                            var partialValidation = await response
                                .Content
                                .DeserializeAsync<PartialValidation>(restClient)
                                .ConfigureAwait(false);

                            // Returns immediately if the validation has been completed or if we should not wait for it

                            if (waitingStrategy == null || !waitingStrategy.WaitForCompletion || partialValidation.Overview.Status == ValidationStatus.Completed)
                            {
                                return await RetrieveValidationFromPartialValidationAsync(partialValidation, cancellationToken)
                                    .ConfigureAwait(false);
                            }

                            return await WaitForCompletionAsync<Validation>(partialValidation.Overview, waitingStrategy,
                                cancellationToken)
                            .ConfigureAwait(false);
                        }

                    case HttpStatusCode.Gone:
                    case HttpStatusCode.NotFound:
                        {
                            return null;
                        }

                    default:
                        {
                            // An unexpected HTTP status code has been received at this point

                            var responseBody = await response
                                .Content
                                .ReadAsStringAsync()
                                .ConfigureAwait(false);

                            throw new VerifaliaException(
                                $"Unexpected HTTP response: {(int)response.StatusCode} {responseBody}");
                        }
                }
            }
        }
    }
}
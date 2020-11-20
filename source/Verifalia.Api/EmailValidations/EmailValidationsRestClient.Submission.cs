/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2020 Cobisi Research
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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Verifalia.Api.EmailValidations.Models;
using Verifalia.Api.Exceptions;

namespace Verifalia.Api.EmailValidations
{
    internal partial class EmailValidationsRestClient
    {
        public Task<Validation> SubmitAsync(string emailAddress, QualityLevelName quality = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));

            return SubmitAsync(new[] {emailAddress},
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
            return SubmitAsync(new[] {entry},
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
                    quality = validationRequest.Quality?.NameOrGuid,
                    deduplication = validationRequest.Deduplication?.NameOrGuid,
                    priority = validationRequest.Priority?.Value,
                    name = validationRequest.Name,
                    // Strips the milliseconds portion from the specified retention period, if any
                    retention = validationRequest.Retention == null
                        ? null
                        : new TimeSpan(validationRequest.Retention.Value.Days,
                                validationRequest.Retention.Value.Hours,
                                validationRequest.Retention.Value.Minutes,
                                validationRequest.Retention.Value.Seconds)
                            .ToString(),
                    
                    // Non-file specific

                    entries = validationRequest.Entries,
                });

            // Send the request to the Verifalia servers

            using (var postedContent = new StringContent(content, Encoding.UTF8, WellKnownMimeContentTypes.ApplicationJson))
            {
                return await SubmitAsync(restClient, postedContent, waitingStrategy, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        
        public async Task<Validation> SubmitAsync(byte[] file, MediaTypeHeaderValue contentType, QualityLevelName quality = default, DeduplicationMode deduplication = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            
            using var stream = new MemoryStream(file);
            return await SubmitAsync(stream,
                    contentType,
                    quality,
                    deduplication,
                    waitingStrategy,
                    cancellationToken)
                .ConfigureAwait(false);
        }
        
        public async Task<Validation> SubmitAsync(FileInfo fileInfo, MediaTypeHeaderValue contentType = default, QualityLevelName quality = default, DeduplicationMode deduplication = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));
            
            using var request = new FileValidationRequest(fileInfo, contentType, quality, deduplication);
            
            return await SubmitAsync(request,
                waitingStrategy,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Validation> SubmitAsync(Stream file, MediaTypeHeaderValue contentType, QualityLevelName quality = default, DeduplicationMode deduplication = default, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));

            using var request = new FileValidationRequest(file, contentType, quality, deduplication, leaveOpen: true);

            return await SubmitAsync(request,
                waitingStrategy,
                cancellationToken)
                .ConfigureAwait(false);
        }
        
        public async Task<Validation> SubmitAsync(FileValidationRequest request, WaitingStrategy waitingStrategy = default, CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var restClient = _restClientFactory.Build();

            // Serialize the validation request (settings only) to JSON

            var settingsContent = restClient
                .Serialize(new
                {
                    quality = request.Quality?.NameOrGuid,
                    deduplication = request.Deduplication?.NameOrGuid,
                    priority = request.Priority?.Value,
                    name = request.Name,
                    // Strips the milliseconds portion from the specified retention period, if any
                    retention = request.Retention == null
                        ? null
                        : new TimeSpan(request.Retention.Value.Days,
                                request.Retention.Value.Hours,
                                request.Retention.Value.Minutes,
                                request.Retention.Value.Seconds)
                            .ToString(),
                    
                    // File-specific
                    
                    startingRow = request.StartingRow,
                    endingRow = request.EndingRow,
                    column = request.Column,
                    sheet = request.Sheet,
                    lineEnding = request.LineEnding,
                    delimiter = request.Delimiter,
                });

            // Send the request to the Verifalia servers

            using (var postedContent = new MultipartFormDataContent())
            using (var postedFileContent = new StreamContent(request.File))
            using (var postedSettingsContent = new StringContent(settingsContent, Encoding.UTF8, WellKnownMimeContentTypes.ApplicationJson))
            {
                postedFileContent.Headers.ContentType = request.ContentType;
                postedContent.Add(postedFileContent, "inputFile",
                    // HACK: Must send a filename, as the backend expects one
                    // see https://github.com/dotnet/aspnetcore/blob/425c196cba530b161b120a57af8f1dd513b96f67/src/Http/Headers/src/ContentDispositionHeaderValueIdentityExtensions.cs#L27
                    "dummy");

                postedSettingsContent.Headers.ContentType = new MediaTypeHeaderValue(WellKnownMimeContentTypes.ApplicationJson);
                postedContent.Add(postedSettingsContent, "settings");

                return await SubmitAsync(restClient, postedContent, waitingStrategy, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        private async Task<Validation> SubmitAsync(IRestClient restClient, HttpContent postedContent, WaitingStrategy waitingStrategy, CancellationToken cancellationToken)
        {
            using (var response = await restClient.InvokeAsync(HttpMethod.Post,
                    "email-validations",
                    queryParams: null,
                    headers: new Dictionary<string, object> {{"Accept", WellKnownMimeContentTypes.ApplicationJson}},
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

                    default:
                    {
                        // An unexpected HTTP status code has been received at this point

                        var responseBody = await response
                            .Content
#if NET5_0
                            .ReadAsStringAsync(cancellationToken)
#else
                            .ReadAsStringAsync()
#endif
                            .ConfigureAwait(false);

                        throw new VerifaliaException(
                            $"Unexpected HTTP response: {(int) response.StatusCode} {responseBody}");
                    }
                }
            }
        }
    }
}
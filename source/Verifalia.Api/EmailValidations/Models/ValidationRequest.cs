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
using System.Threading;
using System.Linq;

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// Represents an email validation request to be submitted against the Verifalia API.
    /// </summary>
    /// <remarks>Once initialized, pass the instance of <see cref="ValidationRequest"/> to the
    /// <see cref="IEmailValidationsRestClient.SubmitAsync(ValidationRequest,WaitingStrategy,CancellationToken)"/> method or one of its
    /// overloads.</remarks>
    public class ValidationRequest : ValidationRequestBase
    {
        /// <summary>
        /// One or more <see cref="ValidationRequestEntry"/> containing with the email addresses to validate, each along
        /// with an optional custom state to be passed back upon completion.
        /// </summary>
        public IReadOnlyCollection<ValidationRequestEntry> Entries { get; }

        /// <summary>
        /// Initializes a <see cref="ValidationRequest"/> to be submitted to the Verifalia email validation engine.
        /// </summary>
        /// <param name="emailAddresses">One or more email addresses to validate.</param>
        /// <param name="quality">An optional <see cref="QualityLevelName"/> referring to the expected results quality for the request.</param>
        /// <param name="deduplication">An optional <see cref="DeduplicationMode"/> to use while determining which email addresses are duplicates.</param>
        /// <param name="priority">An optional <see cref="ValidationPriority"/> (speed) of a validation job, relative to the parent Verifalia account.
        /// <remarks>Setting this value is useful only in the event there are multiple active concurrent validation jobs for the calling Verifalia
        /// account and the current request should be treated differently than the others, with regards to the processing speed.</remarks>
        /// </param>
        public ValidationRequest(IEnumerable<string> emailAddresses, QualityLevelName quality = default, DeduplicationMode deduplication = default, ValidationPriority priority = default)
            : this(emailAddresses.Select(emailAddress => new ValidationRequestEntry(emailAddress)), quality, deduplication, priority)
        {
        }

        /// <summary>
        /// Initializes a <see cref="ValidationRequest"/> to be submitted to the Verifalia email validation engine.
        /// </summary>
        /// <param name="entries">One or more <see cref="ValidationRequestEntry"/> to validate.</param>
        /// <param name="quality">An optional <see cref="QualityLevelName"/> referring to the expected results quality for the request.</param>
        /// <param name="deduplication">An optional <see cref="DeduplicationMode"/> to use while determining which email addresses are duplicates.</param>
        /// <param name="priority">An optional <see cref="ValidationPriority"/> (speed) of a validation job, relative to the parent Verifalia account.
        /// <remarks>Setting this value is useful only in the event there are multiple active concurrent validation jobs for the calling Verifalia
        /// account and the current request should be treated differently than the others, with regards to the processing speed.</remarks>
        /// </param>
        public ValidationRequest(IEnumerable<ValidationRequestEntry> entries, QualityLevelName quality = default, DeduplicationMode deduplication = default, ValidationPriority priority = default)
        {
            var enumeratedEntries = entries.ToArray();

            if (enumeratedEntries.Length == 0)
                throw new ArgumentException("Can't create a validation request out of an empty collection of entries.", nameof(entries));

            Entries = enumeratedEntries;
            Quality = quality;
            Deduplication = deduplication;
            Priority = priority;
        }
    }
}
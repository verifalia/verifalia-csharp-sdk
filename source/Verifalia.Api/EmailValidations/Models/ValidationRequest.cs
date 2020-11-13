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
    public class ValidationRequest
    {
        /// <summary>
        /// A reference to the expected results quality level for this request. Quality levels determine how Verifalia validates
        /// email addresses, including whether and how the automatic reprocessing logic occurs (for transient statuses) and the
        /// verification timeouts settings.
        /// <remarks>Use one of <see cref="QualityLevelName.Standard"/>, <see cref="QualityLevelName.High"/> or <see cref="QualityLevelName.Extreme"/>
        /// values or a custom quality level ID if you have one (custom quality levels are available to premium plans only).</remarks>
        /// </summary>
        public QualityLevelName Quality { get; set; }

        /// <summary>
        /// The priority (speed) of a validation job, relative to the parent Verifalia account. In the event of an account
        /// with many concurrent validation jobs, this value allows to increase the processing speed of a job with respect to the others.
        /// <remarks>The allowed range of values spans from <see cref="ValidationPriority.Lowest"/> (0 - lowest priority) to <see cref="ValidationPriority.Highest"/>
        /// (255 - highest priority), where the midway value  <see cref="ValidationPriority.Normal"/> (127) means normal priority; if not specified,
        /// Verifalia processes all the concurrent validation jobs for an account using the same speed.</remarks>
        /// </summary>
        public ValidationPriority Priority { get; set; }

        /// <summary>
        /// The strategy Verifalia follows while determining which email addresses are duplicates, within a multiple items job.
        /// <remarks>Duplicated items (after the first occurrence) will have the <see cref="ValidationEntryStatus.Duplicate"/> status.</remarks>
        /// </summary>
        public DeduplicationMode Deduplication { get; set; }

        /// <summary>
        /// The maximum data retention period Verifalia observes for this verification job, after which the job will be
        /// automatically deleted. The default value of null forces the service to fall back to the default retention
        /// period for the user or browser app which is submitting the job.
        /// <remarks>A verification job can be deleted anytime prior to its retention period through the
        /// <see cref="EmailValidationsRestClient.DeleteAsync(Guid, CancellationToken)"/> method.</remarks>
        /// <remarks>If set, the retention period must have a value between 5 minutes and 30 days.</remarks>
        /// </summary>
        public TimeSpan? Retention { get; set; }

        /// <summary>
        /// An optional user-defined name for the validation job, for your own reference. The name will be returned
        /// on subsequent API calls and shown on the Verifalia clients area.
        /// </summary>
        public string Name { get; set; }

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
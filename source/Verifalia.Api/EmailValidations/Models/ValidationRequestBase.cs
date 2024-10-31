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
using System.Threading;

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// Base class for email validation requests.
    /// </summary>
    public abstract class ValidationRequestBase
    {
        /// <summary>
        /// A reference to the expected results quality level for this request. Quality levels determine how Verifalia validates
        /// email addresses, including whether and how the automatic reprocessing logic occurs (for transient statuses) and the
        /// verification timeouts settings.
        /// <remarks>Use one of <see cref="QualityLevelName.Standard"/>, <see cref="QualityLevelName.High"/> or <see cref="QualityLevelName.Extreme"/>
        /// values or a custom quality level ID if you have one (custom quality levels are available to premium plans only).</remarks>
        /// </summary>
        public QualityLevelName? Quality { get; set; }

        /// <summary>
        /// Specifies the priority (speed) of a validation job relative to the parent Verifalia account. If there are
        /// multiple concurrent validation jobs in an account, this value allows you to adjust the processing speed of a
        /// specific job in comparison to others.
        /// <remarks>The valid range for this priority spans from <see cref="ValidationPriority.Lowest"/> (0 - lowest
        /// priority) to <see cref="ValidationPriority.Highest"/> (255 - highest priority), with
        /// <see cref="ValidationPriority.Normal"/> (127) representing normal priority. If not specified, Verifalia
        /// processes all concurrent validation jobs for an account at the same speed.
        /// </remarks>
        /// </summary>
        public ValidationPriority? Priority { get; set; }

        /// <summary>
        /// The strategy Verifalia follows while determining which email addresses are duplicates, within a multiple items job.
        /// <remarks>Duplicated items (after the first occurrence) will have the <see cref="ValidationEntryStatus.Duplicate"/> status.</remarks>
        /// </summary>
        public DeduplicationMode? Deduplication { get; set; }

        /// <summary>
        /// Defines the data retention period for this verification job in Verifalia. After this specified period, the
        /// job will be automatically deleted. If set to null, the service defaults to the retention period associated
        /// with the user or browser app submitting the job.
        /// <remarks>A verification job can be deleted at any time before its retention period using the
        /// <see cref="EmailValidationsRestClient.DeleteAsync(Guid, CancellationToken)"/> method. The configured
        /// retention period, if specified, must be within the range of 5 minutes to 30 days.</remarks>
        /// </summary>
        public TimeSpan? Retention { get; set; }

        /// <summary>
        /// Allows to assign an optional custom name to the validation job for personal reference. This name will be
        /// included in subsequent API calls and displayed in the Verifalia client area.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Allows to define an optional URL which Verifalia will invoke once the results for this job are ready.
        /// </summary>
        public CompletionCallback? CompletionCallback { get; set; }
    }
}
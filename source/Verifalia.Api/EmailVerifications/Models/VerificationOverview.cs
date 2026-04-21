/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2026 Cobisi Research
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
using System.Net;
using System.Threading;
using Newtonsoft.Json;

namespace Verifalia.Api.EmailVerifications.Models
{
    /// <summary>
    /// Contains summary information about an email verification job submitted to the Verifalia API.
    /// </summary>
    public class VerificationOverview
    {
        /// <summary>
        /// A server-generated unique identifier for the email verification job.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Date and time when the verification job was submitted to Verifalia.
        /// </summary>
        [JsonProperty("submittedOn")]
        public DateTime SubmittedOn { get; set; }

        /// <summary>
        /// Date and time when the verification job completed processing (if applicable).
        /// </summary>
        [JsonProperty("completedOn")]
        public DateTime? CompletedOn { get; set; }

        /// <summary>
        /// Processing priority relative to other jobs in the same account. If there are
        /// multiple concurrent verification jobs in an account, this value allows you to adjust the processing speed of a
        /// specific job in comparison to others.
        /// <remarks>Priority ranges from <see cref="VerificationPriority.Lowest"/> (0 - lowest
        /// priority) to <see cref="VerificationPriority.Highest"/> (255 - highest priority), with
        /// <see cref="VerificationPriority.Normal"/> (127) as default.
        /// </remarks>
        /// </summary>
        [JsonProperty("priority")]
        public VerificationPriority Priority { get; set; }

        /// <summary>
        /// Optional user-defined reference name for the verification job.
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Unique identifier of the Verifalia user that submitted the job.
        /// </summary>
        /// <remarks>User details can be retrieved using <see cref="Users.IUsersClient.GetAsync(string, CancellationToken)"/>.</remarks>
        [JsonProperty("owner")]
        public string OwnerId { get; set; }

        /// <summary>
        /// Originating IP address of the job submission request.
        /// </summary>
        [JsonProperty("clientIP")]
        public IPAddress ClientIP { get; set; }

        /// <summary>
        /// Date and time when the verification job was created.
        /// </summary>
        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// A reference to the <see cref="QualityLevel"/> against which this job was submitted.
        /// Quality levels determine how Verifalia validates  email addresses, including whether and how the automatic
        /// reprocessing logic occurs (for transient statuses) and the verification timeouts settings.
        /// </summary>
        [JsonProperty("quality")]
        public QualityLevelName Quality { get; set; }

        /// <summary>
        /// The maximum data retention period Verifalia observes for this verification job, after which the job will be
        /// automatically deleted.
        /// <remarks>A verification job can be deleted anytime prior to its retention period through the
        /// <see cref="EmailVerificationsClient.DeleteAsync(string, CancellationToken)"/> method.</remarks>
        /// </summary>
        [JsonProperty("retention")]
        public TimeSpan Retention { get; set; }
        
        /// <summary>
        /// A <see cref="DeduplicationMode"/> option which affected the way Verifalia eventually marked entries as
        /// duplicates upon processing.
        /// </summary>
        [JsonProperty("deduplication")]
        public DeduplicationMode Deduplication { get; set; }

        /// <summary>
        /// The processing <see cref="VerificationStatus"/> for the verification job.
        /// </summary>
        [JsonProperty("status")]
        public VerificationStatus Status { get; set; }

        /// <summary>
        /// The number of entries (email addresses) the verification job contains.
        /// </summary>
        [JsonProperty("noOfEntries")]
        public int NoOfEntries { get; set; }

        /// <summary>
        /// The completion progress of the verification job, if available.
        /// </summary>
        [JsonProperty("progress")]
        public VerificationProgress? Progress { get; set; }
    }
}
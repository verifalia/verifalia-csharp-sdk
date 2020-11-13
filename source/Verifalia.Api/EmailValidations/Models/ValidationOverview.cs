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
using System.Net;
using System.Threading;
using Newtonsoft.Json;

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// Overview information for a <see cref="Validation"/>.
    /// </summary>
    public class ValidationOverview
    {
        /// <summary>
        /// The unique identifier for the validation job.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The date and time this validation job has been submitted to Verifalia.
        /// </summary>
        [JsonProperty("submittedOn")]
        public DateTime SubmittedOn { get; set; }

        /// <summary>
        /// The date and time this validation job has been eventually completed.
        /// </summary>
        [JsonProperty("completedOn")]
        public DateTime? CompletedOn { get; set; }

        /// <summary>
        /// The eventual priority (speed) of the validation job, relative to the parent Verifalia account. In the event of an account
        /// with many concurrent validation jobs, this value allows to increase the processing speed of a job with respect to the others.
        /// </summary>
        /// <remarks>The allowed range of values spans from <see cref="ValidationPriority.Lowest"/> (0 - lowest priority) to
        /// <see cref="ValidationPriority.Highest"/> (255 - highest priority), where the midway value
        /// <see cref="ValidationPriority.Normal"/> (127) means normal priority; if not specified, Verifalia processes all the
        /// concurrent validation jobs for an account using the same priority.</remarks>
        [JsonProperty("priority")]
        public ValidationPriority Priority { get; set; }

        /// <summary>
        /// An optional user-defined name for the validation job, for your own reference.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The unique ID of the Verifalia user who submitted the validation job.
        /// </summary>
        [JsonProperty("owner")]
        public Guid Owner { get; set; }

        /// <summary>
        /// The IP address of the client which submitted the validation job.
        /// </summary>
        [JsonProperty("clientIP")]
        public IPAddress ClientIP { get; set; }

        /// <summary>
        /// The date and time the validation job was created.
        /// </summary>
        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// A reference to the <see cref="QualityLevel"/> this job was validated against.
        /// </summary>
        [JsonProperty("quality")]
        public QualityLevelName Quality { get; set; }

        /// <summary>
        /// The maximum data retention period Verifalia observes for this verification job, after which the job will be
        /// automatically deleted.
        /// <remarks>A verification job can be deleted anytime prior to its retention period through the
        /// <see cref="EmailValidationsRestClient.DeleteAsync(Guid, CancellationToken)"/> method.</remarks>
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
        /// The processing <see cref="ValidationStatus"/> for the validation job.
        /// </summary>
        [JsonProperty("status")]
        public ValidationStatus Status { get; set; }

        /// <summary>
        /// The number of entries the validation job contains.
        /// </summary>
        [JsonProperty("noOfEntries")]
        public int NoOfEntries { get; set; }

        /// <summary>
        /// The eventual completion progress for the validation job.
        /// </summary>
        [JsonProperty("progress")]
        public ValidationProgress Progress { get; set; }
    }
}
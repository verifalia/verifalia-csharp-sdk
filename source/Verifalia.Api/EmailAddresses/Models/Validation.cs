using System;
using System.Collections.Generic;

namespace Verifalia.Api.EmailAddresses.Models
{
    /// <summary>
    /// Represents a snapshot of an email validation batch.
    /// </summary>
    public class Validation
    {
        /// <summary>
        /// The unique identifier of the email validation batch.
        /// </summary>
        public Guid UniqueID { get; set; }

        /// <summary>
        /// The status of this batch.
        /// </summary>
        public ValidationStatus Status { get; set; }
        
        /// <summary>
        /// The internal version of the Verifalia engine which provided this snapshot.
        /// </summary>
        public string EngineVersion { get; set; }

        /// <summary>
        /// The date the batch has been submitted to Verifalia.
        /// </summary>
        public DateTime? SubmittedOn { get; set; }

        /// <summary>
        /// The date the batch has been completed.
        /// </summary>
        public DateTime? CompletedOn { get; set; }

        /// <summary>
        /// A list of results for the email validation batch.
        /// </summary>
        public List<ValidationEntry> Entries { get; set; }

        /// <summary>
        /// The completion progress for the batch.
        /// </summary>
        public ValidationProgress Progress { get; set; }
    }
}
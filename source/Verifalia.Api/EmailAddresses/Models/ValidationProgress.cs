using System;

namespace Verifalia.Api.EmailAddresses.Models
{
    /// <summary>
    /// Represents the completion progress of an email validation batch.
    /// </summary>
    public class ValidationProgress
    {
        /// <summary>
        /// The number of total entries of the batch.
        /// </summary>
        public int NoOfTotalEntries { get; set; }

        /// <summary>
        /// The number of completed entries of the batch.
        /// </summary>
        public int NoOfCompletedEntries { get; set; }

        /// <summary>
        /// The estimated time remaining to complete the whole validation job, if available.
        /// </summary>
        public TimeSpan? EstimatedTimeRemaining { get; set; }

        /// <summary>
        /// Returns the number of pending entries of the batch.
        /// </summary>
        public int NoOfPendingEntries
        {
            get
            {
                return NoOfTotalEntries - NoOfCompletedEntries;
            }
        }

        /// <summary>
        /// Returns the progress percentage of the batch.
        /// </summary>
        public double Percentage
        {
            get
            {
                return 100d * NoOfCompletedEntries / NoOfTotalEntries;
            }
        }
    }
}
using System;

namespace Verifalia.Api
{
    /// <summary>
    /// Represent the waiting options for a submission to the Verifalia API.
    /// </summary>
    public class WaitForCompletionOptions
    {
        /// <summary>
        /// Instructs the client to avoid waiting for the completion of the submission at the Verifalia side; will
        /// return it in pending status, if it has not been completed yet.
        /// </summary>
        public static WaitForCompletionOptions DontWait = new WaitForCompletionOptions();

        /// <summary>
        /// The timeout to obey while waiting for completion.
        /// </summary>
        public TimeSpan Timeout { get; private set; }

        /// <summary>
        /// The polling interval for the completion check.
        /// </summary>
        public TimeSpan PollingInterval { get; set; }

        /// <summary>
        /// Default interval between subsequent polling requests.
        /// </summary>
        static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromSeconds(5);

        private WaitForCompletionOptions()
        {
            // Private constructor, used by DontWait
        }

        public WaitForCompletionOptions(TimeSpan timeout)
            : this(timeout, DefaultPollingInterval)
        {
        }

        public WaitForCompletionOptions(TimeSpan timeout, TimeSpan pollingInterval)
        {
            Timeout = timeout;
            PollingInterval = pollingInterval;
        }
    }
}
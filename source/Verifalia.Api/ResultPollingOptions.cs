using System;

namespace Verifalia.Api
{
    /// <summary>
    /// Represent the results polling options for a submission to the Verifalia API.
    /// </summary>
    public class ResultPollingOptions
    {
        /// <summary>
        /// Default interval between subsequent polling requests.
        /// </summary>
        static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Default maximum number of polling requests.
        /// </summary>
        static readonly int DefaultMaxPollingCount = Int32.MaxValue;

        /// <summary>
        /// Instructs the client to avoid waiting for the completion of the submission at the Verifalia side; will
        /// return it in pending status, if it has not been completed yet.
        /// </summary>
        public static ResultPollingOptions NoPolling = new ResultPollingOptions(0);

        /// <summary>
        /// Instructs the client to automatically wait for the completion of the submitted job, with a polling interval of 5 seconds.
        /// </summary>
        public static ResultPollingOptions WaitUntilCompleted = new ResultPollingOptions();

        /// <summary>
        /// The polling interval for the completion check.
        /// </summary>
        public TimeSpan PollingInterval { get; set; }

        private int _maxPollingCount;

        /// <summary>
        /// The maximum number of polling requests before giving up and returning the job in pending status.
        /// </summary>
        public int MaxPollingCount
        {
            get => _maxPollingCount;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _maxPollingCount = value;
            }
        }

        /// <summary>
        /// If true, throws an exception if the client can't complete a validation job even after the configured maximum number
        /// of allowed polling iterations. Default is false.
        /// </summary>
        public bool ThrowIfNotCompleted { get; set; }

        public ResultPollingOptions()
            : this(DefaultPollingInterval, DefaultMaxPollingCount)
        {
        }

        public ResultPollingOptions(TimeSpan pollingInterval)
            : this(pollingInterval, DefaultMaxPollingCount)
        {
        }

        public ResultPollingOptions(int maxPollingCount)
            : this(DefaultPollingInterval, maxPollingCount)
        {
        }

        public ResultPollingOptions(TimeSpan pollingInterval, int maxPollingCount)
        {
            PollingInterval = pollingInterval;
            MaxPollingCount = maxPollingCount;
        }
    }
}
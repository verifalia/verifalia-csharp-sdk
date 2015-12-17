namespace Verifalia.Api.EmailAddresses.Models
{
    /// <summary>
    /// The possible statuses for an email validation batch.
    /// </summary>
    public enum ValidationStatus
    {
        /// <summary>
        /// Unknown status, due to a value reported by the API which is missing in this SDK.
        /// </summary>
        Unknown,

        /// <summary>
        /// The email validation batch is being processed by Verifalia.
        /// </summary>
        Pending,

        /// <summary>
        /// The email validation batch has been completed and its results are available.
        /// </summary>
        Completed
    }
}

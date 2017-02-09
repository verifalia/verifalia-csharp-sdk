using System;

namespace Verifalia.Api.AccountBalance.Models
{
    /// <summary>
    /// Represents a Verifalia account balance.
    /// </summary>
    public class AccountBalance
    {
        /// <summary>
        /// The number of non-expiring credits of the account.
        /// </summary>
        public decimal Credits { get; set; }

        /// <summary>
        /// The number of free daily credits of the account.
        /// </summary>
        public decimal? FreeCredits { get; set; }

        /// <summary>
        /// The time required for the free daily credits to reset.
        /// </summary>
        public TimeSpan? FreeCreditsResetIn { get; set; }
    }
}

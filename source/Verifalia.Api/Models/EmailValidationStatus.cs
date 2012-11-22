using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verifalia.Api.Models
{
    /// <summary>
    /// The possible statuses for an email validation batch.
    /// </summary>
    public enum EmailValidationStatus
    {
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

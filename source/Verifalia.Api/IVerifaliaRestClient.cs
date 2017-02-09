using System;
using Verifalia.Api.AccountBalance;
using Verifalia.Api.EmailAddresses;

namespace Verifalia.Api
{
    public interface IVerifaliaRestClient
    {
        /// <summary>
        /// Verifalia API version to use when making requests.
        /// </summary>
        string ApiVersion { get; set; }

        /// <summary>
        /// Base URIs of Verifalia API (defaults to https://api-1.verifalia.com/ and https://api-2.verifalia.com). Do not use this property
        /// unless instructed to so by the Verifalia support team.
        /// </summary>
        Uri[] BaseUris { get; set; }

        /// <summary>
        /// Allows to submit and manage email validations using the Verifalia service.
        /// </summary>
        IValidationRestClient EmailValidations { get; }

        /// <summary>
        /// Allows to manage the credits for the Verifalia account.
        /// </summary>
        IAccountBalanceRestClient AccountBalance { get; }
    }
}
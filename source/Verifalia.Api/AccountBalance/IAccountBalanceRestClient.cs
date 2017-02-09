using System.Threading;
using System.Threading.Tasks;

namespace Verifalia.Api.AccountBalance
{
    public interface IAccountBalanceRestClient
    {
        /// <summary>
        /// Returns the balance for the calling Verifalia account.
        /// </summary>
        Models.AccountBalance Query();

        /// <summary>
        /// Returns the balance for the calling Verifalia account.
        /// </summary>
        Task<Models.AccountBalance> QueryAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
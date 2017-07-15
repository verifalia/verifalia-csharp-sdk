using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace Verifalia.Api
{
    public interface IRestClient
    {
        Task<HttpResponseMessage> InvokeAsync(HttpMethod verb, string resource, HttpContent content = null, CancellationToken cancellationToken = default(CancellationToken));
        HttpContent SerializeContent(object obj);
        Task<T> DeserializeContentAsync<T>(HttpResponseMessage message);
    }
}
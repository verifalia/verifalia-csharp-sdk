using RestSharp;

namespace Verifalia.Api
{
    public interface IRestClientFactory
    {
        RestClient Build();
    }
}
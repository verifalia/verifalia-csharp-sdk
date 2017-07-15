namespace Verifalia.Api
{
    public interface IRestClientFactory
    {
        IRestClient Build();
    }
}
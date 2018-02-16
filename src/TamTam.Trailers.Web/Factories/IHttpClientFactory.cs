namespace TamTam.Trailers.Web.Factories
{
    using System.Net.Http;

    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
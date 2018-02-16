namespace TamTam.Trailers.Web.Factories
{
    using System.Net.Http;

    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient client;

        public HttpClientFactory()
        {
            client = new HttpClient();
        }

        public HttpClient Create()
        {
            return client;
        }
    }
}
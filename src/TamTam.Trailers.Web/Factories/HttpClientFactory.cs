namespace TamTam.Trailers.Web.Factories
{
    using System.Net.Http;
    using Microsoft.Extensions.Logging;
    using TamTam.Trailers.Web.Logging;

    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient client;

        public HttpClientFactory(ILogger<HttpClient> logger)
        {
            var handler = new LoggingHandler(logger);
            client = new HttpClient(handler);
        }

        public HttpClient Create()
        {
            return client;
        }
    }
}
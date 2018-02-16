namespace TamTam.Trailers.Web.Services.Movies
{
    using System.Net.Http;
    using Moq;
    using TamTam.Trailers.Web.Factories;
    using TamTam.Trailers.Web.Infrastructure;

    public abstract class MovieServiceTests
    {
        protected readonly Mock<IHttpClientFactory> Factory;

        protected MovieServiceTests()
        {
            Factory = new Mock<IHttpClientFactory>();
        }

        protected void SetupResponse(string json)
        {
            var handler = new TestHttpMessageHandler(json);
            var client = new HttpClient(handler);
            Factory.Setup(x => x.Create())
                .Returns(client);
        }
    }
}
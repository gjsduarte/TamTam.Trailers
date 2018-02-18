namespace TamTam.Trailers.Infrastructure.Services
{
    using System.Net.Http;
    using Moq;
    using TamTam.Trailers.Infrastructure.Factories;

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
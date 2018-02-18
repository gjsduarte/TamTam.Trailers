namespace TamTam.Trailers.Infrastructure.Services
{
    using System.Net.Http;

    using Microsoft.Extensions.Caching.Distributed;

    using Moq;

    using TamTam.Trailers.Infrastructure.Factories;

    public abstract class MovieServiceTests
    {
        #region Constructors and Destructors

        protected MovieServiceTests()
        {
            Factory = new Mock<IHttpClientFactory>();
            Cache = new Mock<IDistributedCache>();
        }

        #endregion

        #region Public Properties

        public Mock<IDistributedCache> Cache { get; set; }

        public Mock<IHttpClientFactory> Factory { get; set; }

        #endregion

        #region Methods

        protected void SetupResponse(string json)
        {
            var handler = new TestHttpMessageHandler(json);
            var client = new HttpClient(handler);
            Factory.Setup(x => x.Create()).Returns(client);
        }

        #endregion
    }
}
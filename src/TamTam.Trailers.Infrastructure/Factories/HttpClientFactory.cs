namespace TamTam.Trailers.Infrastructure.Factories
{
    using System.Net.Http;
    using Microsoft.Extensions.Logging;
    using TamTam.Trailers.Infrastructure.Logging;

    public class HttpClientFactory : IHttpClientFactory
    {
        #region Fields

        private readonly HttpClient client;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientFactory"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public HttpClientFactory(ILogger<HttpClient> logger)
        {
            var handler = new LoggingHandler(logger);
            client = new HttpClient(handler);
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public HttpClient Create()
        {
            return client;
        }

        #endregion
    }
}
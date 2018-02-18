namespace TamTam.Trailers.Infrastructure.Logging
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class LoggingHandler : HttpClientHandler
    {
        #region Fields

        private readonly ILogger<HttpClient> logger;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LoggingHandler(ILogger<HttpClient> logger)
        {
            this.logger = logger;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"Executing request {request}");
                return base.SendAsync(request, cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"Error executing request {request}");
                throw;
            }
        }

        #endregion
    }
}
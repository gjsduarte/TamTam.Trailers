namespace TamTam.Trailers.Web.Logging
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class LoggingHandler : HttpClientHandler
    {
        private readonly ILogger<HttpClient> logger;

        public LoggingHandler(ILogger<HttpClient> logger)
        {
            this.logger = logger;
        }

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
    }
}
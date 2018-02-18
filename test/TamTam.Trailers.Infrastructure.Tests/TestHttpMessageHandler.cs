namespace TamTam.Trailers.Infrastructure
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class TestHttpMessageHandler : HttpMessageHandler
    {
        #region Fields

        private readonly string json;

        #endregion

        #region Constructors and Destructors

        public TestHttpMessageHandler(string json)
        {
            this.json = json;
        }

        #endregion

        #region Methods

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }

        #endregion
    }
}
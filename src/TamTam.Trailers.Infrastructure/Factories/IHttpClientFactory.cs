namespace TamTam.Trailers.Infrastructure.Factories
{
    using System.Net.Http;

    public interface IHttpClientFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a new HttpClient instance.
        /// </summary>
        /// <returns>a new HttpClient instance.</returns>
        HttpClient Create();

        #endregion
    }
}
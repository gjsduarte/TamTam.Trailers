namespace TamTam.Trailers.Infrastructure.Extensions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class HttpClientExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the HttpClient response parsed as json to a dynamic object.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="uri">The URI.</param>
        /// <returns>A dynamic object</returns>
        public static async Task<dynamic> GetAsJson(this HttpClient client, string uri)
        {
            var json = await client.GetStringAsync(uri);
            return JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// Gets the HttpClient response parsed as json to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">The client.</param>
        /// <param name="uri">The URI.</param>
        /// <returns>An object of the speficied type</returns>
        public static async Task<T> GetAsJson<T>(this HttpClient client, string uri)
        {
            var json = await client.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion
    }
}
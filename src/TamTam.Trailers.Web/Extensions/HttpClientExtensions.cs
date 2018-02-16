namespace TamTam.Trailers.Web.Extensions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class HttpClientExtensions
    {
        public static async Task<dynamic> GetAsJson(this HttpClient client, string uri)
        {
            var json = await client.GetStringAsync(uri);
            return JsonConvert.DeserializeObject(json);
        }

        public static async Task<T> GetAsJson<T>(this HttpClient client, string uri)
        {
            var json = await client.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
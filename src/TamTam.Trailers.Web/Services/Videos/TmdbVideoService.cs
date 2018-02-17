namespace TamTam.Trailers.Web.Services.Videos
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using TamTam.Trailers.Web.Extensions;
    using TamTam.Trailers.Web.Factories;
    using TamTam.Trailers.Web.Model;
    using TamTam.Trailers.Web.Options;

    public class TmdbVideoService : IVideoService
    {
        private readonly IHttpClientFactory factory;
        private readonly TmdbOptions options;

        public TmdbVideoService(IHttpClientFactory factory, IOptions<TmdbOptions> options)
        {
            this.factory = factory;
            this.options = options.Value;
        }

        public async Task<IEnumerable<Video>> Search(string id)
        {
            var client = factory.Create();
            var uri = $"{options.Address}movie/{id}/videos?api_key={options.ApiKey}";
            var response = await client.GetAsJson(uri);
            
            var videos = new List<Video>();
            foreach (var result in response.results)
            {
                if (result.type != "Trailer") continue;
                var movie = ParseVideo(result);
                videos.Add(movie);
            }

            return videos;
        }

        private static Video ParseVideo(dynamic result)
        {
            return new Video
            {
                Name = result.name,
                Key = result.key,
                Type = Enum.Parse<VideoType>(result.site.ToString(), true)
            };
        }
    }
}
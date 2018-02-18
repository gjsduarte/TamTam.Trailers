namespace TamTam.Trailers.Services.Tmdb
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;
    
    using TamTam.Trailers.Infrastructure;
    using TamTam.Trailers.Infrastructure.Extensions;
    using TamTam.Trailers.Infrastructure.Factories;
    using TamTam.Trailers.Infrastructure.Model;
    using TamTam.Trailers.Infrastructure.Services;
    using TamTam.Trailers.Services.Tmdb.Options;

    public class TmdbVideoService : IVideoService
    {
        #region Fields

        private readonly IHttpClientFactory factory;
        private readonly IDistributedCache cache;
        private readonly TmdbOptions options;
        private readonly DistributedCacheEntryOptions cacheOptions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TmdbVideoService"/> class.
        /// </summary>
        /// <param name="factory">The http client factory.</param>
        /// <param name="cache">The current cache store.</param>
        /// <param name="options">The options.</param>
        public TmdbVideoService(IHttpClientFactory factory, IDistributedCache cache, IOptions<TmdbOptions> options)
        {
            this.factory = factory;
            this.cache = cache;
            this.options = options.Value;
            
            // TODO: Read this from options
            cacheOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5d)
            };
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public async Task<IEnumerable<Video>> Search(string id)
        {
            var uri = $"{options.Address}movie/{id}/videos?api_key={options.ApiKey}";

            // Try getting the result from the cache
            var videos = await cache.GetAsJsonAsync<IEnumerable<Video>>(uri).ToList();
            if (videos == null)
            {
                // Fetch the results
                var client = factory.Create();
                var response = await Policies.Retry.ExecuteAsync(() => client.GetAsJson(uri));

                // Parse the results
                videos = new List<Video>();
                foreach (var result in response.results)
                {
                    if (result.type != "Trailer") continue;

                    Video video = ParseVideo(result);
                    videos.Add(video);
                }
                
                // Store the values in the cache
                await cache.SetAsJsonAsync(uri, videos, cacheOptions);
            }

            return videos;
        }

        #endregion

        #region Methods

        private static Video ParseVideo(dynamic result)
        {
            return new Video
            {
                Name = result.name,
                Key = result.key,
                Type = Enum.Parse<VideoType>(result.site.ToString(), true)
            };
        }

        #endregion
    }
}
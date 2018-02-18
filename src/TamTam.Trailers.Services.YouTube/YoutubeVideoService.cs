namespace TamTam.Trailers.Services.YouTube
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    using Google.Apis.Services;
    using Google.Apis.YouTube.v3;
    
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;
    
    using TamTam.Trailers.Infrastructure;
    using TamTam.Trailers.Infrastructure.Extensions;
    using TamTam.Trailers.Infrastructure.Model;
    using TamTam.Trailers.Infrastructure.Services;
    using TamTam.Trailers.Services.YouTube.Options;

    public class YoutubeVideoService : IVideoService
    {
        #region Fields

        private readonly IMovieService movieService;
        private readonly IDistributedCache cache;
        private readonly YouTubeService youtubeService;
        private readonly DistributedCacheEntryOptions cacheOptions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="YoutubeVideoService" /> class.
        /// </summary>
        /// <param name="movieService">The movie service.</param>
        /// <param name="cache">The current cache store.</param>
        /// <param name="options">The options.</param>
        public YoutubeVideoService(IMovieService movieService, IDistributedCache cache, IOptions<YoutubeOptions> options)
        {
            this.movieService = movieService;
            this.cache = cache;
            // TODO: Read this from options
            cacheOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5d)
            };

            var initializer = new BaseClientService.Initializer
            {
                ApiKey = options.Value.ApiKey,
                ApplicationName = options.Value.ApplicationName
            };

            youtubeService = new YouTubeService(initializer);
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public async Task<IEnumerable<Video>> Search(string id)
        {
            // Fetch the movie with the specified id
            var movie = await movieService.Get(id);

            // Yeah, not very intelligent, but it does the job :)
            var query = $"{movie.Title} {movie.Year} trailer";
            
            // Try getting the result from the cache
            var videos = await cache.GetAsJsonAsync<IEnumerable<Video>>(query).ToList();
            if (videos == null)
            {
                var searchListRequest = youtubeService.Search.List("snippet");
                searchListRequest.Q = query;
                searchListRequest.MaxResults = 50;
                searchListRequest.Type = "video";

                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = await Policies.Retry.ExecuteAsync(() => searchListRequest.ExecuteAsync());

                // Parse the results
                videos = searchListResponse.Items.Select(searchResult => new Video
                {
                    Name = searchResult.Snippet.Title,
                    Key = searchResult.Id.VideoId,
                    Type = VideoType.YouTube
                }).ToList();    
                
                // Store the values in the cache
                await cache.SetAsJsonAsync(query, videos, cacheOptions);
            }

            
            return videos;
        }

        #endregion
    }
}
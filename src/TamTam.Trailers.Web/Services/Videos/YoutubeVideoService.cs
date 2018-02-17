namespace TamTam.Trailers.Web.Services.Videos
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Google.Apis.Services;
    using Google.Apis.YouTube.v3;

    using Microsoft.Extensions.Options;

    using TamTam.Trailers.Web.Model;
    using TamTam.Trailers.Web.Options;
    using TamTam.Trailers.Web.Services.Movies;

    public class YoutubeVideoService : IVideoService
    {
        #region Fields

        private readonly IMovieService movieService;
        private readonly YouTubeService youtubeService;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="YoutubeVideoService" /> class.
        /// </summary>
        /// <param name="movieService">The movie service.</param>
        /// <param name="options">The options.</param>
        public YoutubeVideoService(IMovieService movieService, IOptions<YoutubeOptions> options)
        {
            this.movieService = movieService;

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

            var searchListRequest = youtubeService.Search.List("snippet");
            // Yeah, not very intelligent, but it does the job :)
            searchListRequest.Q = $"{movie.Title} {movie.Year} trailer";
            searchListRequest.MaxResults = 50;
            searchListRequest.Type = "video";

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await Policies.Retry.ExecuteAsync(() => searchListRequest.ExecuteAsync());

            // Parse the results
            return searchListResponse.Items.Select(searchResult => new Video
            {
                Name = searchResult.Snippet.Title,
                Key = searchResult.Id.VideoId,
                Type = VideoType.YouTube
            });
        }

        #endregion
    }
}
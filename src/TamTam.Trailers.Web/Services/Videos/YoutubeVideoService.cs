namespace TamTam.Trailers.Web.Services.Videos
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Google.Apis.Services;
    using Google.Apis.YouTube.v3;
    using Microsoft.Extensions.Options;
    using TamTam.Trailers.Web.Model;
    using TamTam.Trailers.Web.Options;
    using TamTam.Trailers.Web.Services.Movies;

    public class YoutubeVideoService : IVideoService
    {
        private readonly IMovieService movieService;
        private readonly YouTubeService youtubeService;

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

        public async Task<IEnumerable<Video>> Search(string id)
        {
            var movie = await movieService.Get(id);
            
            var searchListRequest = youtubeService.Search.List("snippet");
            // Yeah, not very intelligent, but it does the job :)
            searchListRequest.Q = $"{movie.Title} {movie.Year} trailer";
            searchListRequest.MaxResults = 50;
            searchListRequest.Type = "video";
            
            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();
            
            var videos = new List<Video>();
            foreach (var searchResult in searchListResponse.Items)
            {
                var video = new Video
                {
                    Name = searchResult.Snippet.Title,
                    Key = searchResult.Id.VideoId,
                    Type = VideoType.YouTube
                };
                videos.Add(video);
            }

            return videos;
        }
    }
}
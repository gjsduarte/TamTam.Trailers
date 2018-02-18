namespace TamTam.Trailers.Services.Omdb
{
    using System.Collections.Generic;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using TamTam.Trailers.Infrastructure;
    using TamTam.Trailers.Infrastructure.Extensions;
    using TamTam.Trailers.Infrastructure.Factories;
    using TamTam.Trailers.Infrastructure.Model;
    using TamTam.Trailers.Infrastructure.Services;
    using TamTam.Trailers.Services.Omdb.Options;

    public class OmdbMovieService : IMovieService
    {
        #region Fields

        private readonly IHttpClientFactory factory;
        private readonly OmdbOptions options;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OmdbMovieService"/> class.
        /// </summary>
        /// <param name="factory">The http client factory.</param>
        /// <param name="options">The options.</param>
        public OmdbMovieService(IHttpClientFactory factory, IOptions<OmdbOptions> options)
        {
            this.factory = factory;
            this.options = options.Value;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public async Task<Movie> Get(string id)
        {
            var uri = $"{options.Address}?apikey={options.ApiKey}&i={id}&plot=full";

            // Fetch the movie
            var client = factory.Create();
            var response = await Policies.Retry.ExecuteAsync(() => client.GetAsJson(uri));

            // Parse the movie
            return ParseMovie(response);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Movie>> Search(string query)
        {
            // Encode the search query before using it
            var encoded = UrlEncoder.Default.Encode(query);
            var uri = $"{options.Address}?apikey={options.ApiKey}&s={encoded}";

            // Fetch the results
            var client = factory.Create();
            var response = await Policies.Retry.ExecuteAsync(() => client.GetAsJson(uri));

            // Parse the results
            var movies = new List<Movie>();
            foreach (var result in response.Search)
            {
                var movie = ParseMovie(result);
                movies.Add(movie);
            }

            return movies;
        }

        #endregion

        #region Methods

        private static Movie ParseMovie(dynamic result)
        {
            return new Movie
            {
                Id = result.imdbID,
                Title = result.Title,
                Year = ParseYear(result.Year.ToString()),
                Poster = ParsePoster(result.Poster?.ToString()),
                Plot = result.Plot,
                ImdbId = result.imdbID
            };
        }

        private static string ParsePoster(string str)
        {
            return string.IsNullOrWhiteSpace(str) || !str.StartsWith("http") ? null : str;
        }

        private static int ParseYear(string str)
        {
            return int.Parse(str.Trim().Left(4));
        }

        #endregion
    }
}
namespace TamTam.Trailers.Web.Services.Movies
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;

    using TamTam.Trailers.Web.Extensions;
    using TamTam.Trailers.Web.Factories;
    using TamTam.Trailers.Web.Model;
    using TamTam.Trailers.Web.Options;

    public class TmdbMovieService : IMovieService
    {
        #region Fields

        private readonly IHttpClientFactory factory;
        private readonly TmdbOptions options;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TmdbMovieService" /> class.
        /// </summary>
        /// <param name="factory">The http client factory.</param>
        /// <param name="options">The options.</param>
        public TmdbMovieService(IHttpClientFactory factory, IOptions<TmdbOptions> options)
        {
            this.factory = factory;
            this.options = options.Value;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public async Task<Movie> Get(string id)
        {
            var uri = $"{options.Address}movie/{id}?api_key={options.ApiKey}";

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
            var uri = $"{options.Address}search/movie?api_key={options.ApiKey}&query={encoded}";

            // Fetch the results
            var client = factory.Create();
            var response = await Policies.Retry.ExecuteAsync(() => client.GetAsJson(uri));

            // Parse the results
            var movies = new List<Movie>();
            foreach (var result in response.results)
            {
                var movie = ParseMovie(result);
                movies.Add(movie);
            }

            return movies;
        }

        #endregion

        #region Methods

        private static string ParseImage(string str, int width)
        {
            return string.IsNullOrWhiteSpace(str) ? null : $"http://image.tmdb.org/t/p/w{width}{str}";
        }

        private static Movie ParseMovie(dynamic result)
        {
            return new Movie
            {
                Id = result.id,
                Title = result.title,
                Rating = result.vote_average,
                Poster = ParseImage(result.poster_path?.ToString(), 185),
                Year = ParseYear(result.release_date?.ToString()),
                Plot = result.overview,
                Backdrop = ParseImage(result.backdrop_path?.ToString(), 1920),
                ImdbId = result.imdb_id
            };
        }

        private static int? ParseYear(string str)
        {
            return DateTime.TryParseExact(str,
                provider: CultureInfo.InvariantCulture,
                style: DateTimeStyles.None,
                format: "yyyy-MM-dd",
                result: out var date)
                ? date.Year
                : (int?)null;
        }

        #endregion
    }
}
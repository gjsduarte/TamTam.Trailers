namespace TamTam.Trailers.Services.Tmdb
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;
    
    using TamTam.Trailers.Infrastructure;
    using TamTam.Trailers.Infrastructure.Extensions;
    using TamTam.Trailers.Infrastructure.Factories;
    using TamTam.Trailers.Infrastructure.Model;
    using TamTam.Trailers.Infrastructure.Services;
    using TamTam.Trailers.Services.Tmdb.Options;

    public class TmdbMovieService : IMovieService
    {
        #region Fields

        private readonly IHttpClientFactory factory;
        private readonly IDistributedCache cache;
        private readonly TmdbOptions options;
        private readonly DistributedCacheEntryOptions cacheOptions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TmdbMovieService" /> class.
        /// </summary>
        /// <param name="factory">The http client factory.</param>
        /// <param name="cache">The current cache store.</param>
        /// <param name="options">The options.</param>
        public TmdbMovieService(IHttpClientFactory factory, IDistributedCache cache, IOptions<TmdbOptions> options)
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
        public async Task<Movie> Get(string id)
        {
            var uri = $"{options.Address}movie/{id}?api_key={options.ApiKey}";

            // Try getting the result from the cache
            var movie = await cache.GetAsJsonAsync<Movie>(uri);
            if (movie == null)
            {
                // Fetch the movie from the API
                var client = factory.Create();
                var response = await Policies.Retry.ExecuteAsync(() => client.GetAsJson(uri));
                
                // Parse the movie
                movie = ParseMovie(response);
                
                // Store the value in the cache
                await cache.SetAsJsonAsync(uri, movie, cacheOptions);
            }

            return movie;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Movie>> Search(string query)
        {
            // Encode the search query before using it
            var encoded = UrlEncoder.Default.Encode(query);
            var uri = $"{options.Address}search/movie?api_key={options.ApiKey}&query={encoded}";

            // Try getting the result from the cache
            var movies = await cache.GetAsJsonAsync<IEnumerable<Movie>>(uri).ToList();
            if (movies == null)
            {
                // Fetch the results
                var client = factory.Create();
                var response = await Policies.Retry.ExecuteAsync(() => client.GetAsJson(uri));

                // Parse the results
                movies = new List<Movie>();
                foreach (var result in response.results)
                {
                    Movie movie = ParseMovie(result);
                    movies.Add(movie);
                }
                
                // Store the values in the cache
                await cache.SetAsJsonAsync(uri, movies, cacheOptions);
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
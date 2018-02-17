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
        private readonly IHttpClientFactory factory;
        private readonly TmdbOptions options;

        public TmdbMovieService(IHttpClientFactory factory, IOptions<TmdbOptions> options)
        {
            this.factory = factory;
            this.options = options.Value;
        }

        public async Task<Movie> Get(string id)
        {
            var client = factory.Create();
            var uri = $"{options.Address}movie/{id}?api_key={options.ApiKey}";
            var response = await client.GetAsJson(uri);
            return ParseMovie(response);
        }

        async Task<IEnumerable<Movie>> IMovieService.Search(string query)
        {
            var client = factory.Create();
            var encoded = UrlEncoder.Default.Encode(query);
            var uri = $"{options.Address}search/movie?api_key={options.ApiKey}&query={encoded}";
            var response = await client.GetAsJson(uri);

            var movies = new List<Movie>();
            foreach (var result in response.results)
            {
                var movie = ParseMovie(result);
                movies.Add(movie);
            }

            return movies;
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

        private static string ParseImage(string str, int width)
        {
            return string.IsNullOrWhiteSpace(str) ? null : $"http://image.tmdb.org/t/p/w{width}{str}";
        }

        private static int? ParseYear(string str)
        {
            return DateTime.TryParseExact(
                str,
                provider: CultureInfo.InvariantCulture,
                style: DateTimeStyles.None,
                format: "yyyy-MM-dd",
                result: out var date)
                ? date.Year
                : (int?) null;
        }
    }
}
namespace TamTam.Trailers.Web.Services.Movies.Tmdb
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using TamTam.Trailers.Web.Extensions;
    using TamTam.Trailers.Web.Factories;
    using TamTam.Trailers.Web.Services.Movies.Model;

    public class TmdbMovieService : IMovieService
    {
        private readonly IHttpClientFactory factory;
        private readonly TmdbOptions options;

        public TmdbMovieService(IHttpClientFactory factory, IOptions<TmdbOptions> options)
        {
            this.factory = factory;
            this.options = options.Value;
        }

        public async Task<IEnumerable<Movie>> Search(string query)
        {
            var client = factory.Create();
            var encoded = UrlEncoder.Default.Encode(query);
            var response = await client.GetAsJson(
                $"{options.Address}search/movie?api_key={options.ApiKey}&query={encoded}");

            var movies = new List<Movie>();
            foreach (var result in response.results)
            {
                var movie = ParseMovie(result);
                movies.Add(movie);
            }

            return movies;
        }

        public async Task<Movie> Get(string id)
        {
            var client = factory.Create();
            var response = await client.GetAsJson(
                $"{options.Address}movie/{id}?api_key={options.ApiKey}");
            return ParseMovie(response);
        }

        private static Movie ParseMovie(dynamic result)
        {
            var movie = new Movie
            {
                Id = result.id,
                Title = result.title,
                Rating = result.vote_average,
                Poster = ParsePoster(result.poster_path?.ToString()),
                Year = ParseYear(result.release_date?.ToString()),
                Plot = result.overview
            };
            return movie;
        }

        private static string ParsePoster(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? null : $"http://image.tmdb.org/t/p/w185{str}";
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
namespace TamTam.Trailers.Web.Services.Movies.Omdb
{
    using System.Collections.Generic;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using TamTam.Trailers.Web.Extensions;
    using TamTam.Trailers.Web.Factories;
    using TamTam.Trailers.Web.Services.Movies.Model;

    public class OmdbMovieService : IMovieService
    {
        private readonly IHttpClientFactory factory;
        private readonly OmdbOptions options;

        public OmdbMovieService(IHttpClientFactory factory, IOptions<OmdbOptions> options)
        {
            this.factory = factory;
            this.options = options.Value;
        }

        public async Task<IEnumerable<Movie>> Search(string query)
        {
            var client = factory.Create();
            var encoded = UrlEncoder.Default.Encode(query);
            var response = await client.GetAsJson($"{options.Address}?apikey={options.ApiKey}&s={encoded}");
            var movies = new List<Movie>();
            foreach (var result in response.Search)
            {
                var movie = ParseMovie(result);
                movies.Add(movie);
            }
            return movies;
        }

        public async Task<Movie> Get(string id)
        {
            var client = factory.Create();
            var response = await client.GetAsJson($"{options.Address}?apikey={options.ApiKey}&i={id}");
            return ParseMovie(response);
        }

        private static Movie ParseMovie(dynamic result)
        {
            return new Movie
            {
                Id = result.imdbID,
                Title = result.Title,
                Year = ParseYear(result.Year.ToString()),
                Poster = ParsePoster(result.Poster?.ToString()),
                Plot = result.Plot
            };
        }

        private static string ParsePoster(string str)
        {
            return string.IsNullOrWhiteSpace(str) || !str.StartsWith("http")
                ? null
                : str;
        }

        private static int ParseYear(string str)
        {
            return int.Parse(str.Trim().Left(4));
        }
    }
}
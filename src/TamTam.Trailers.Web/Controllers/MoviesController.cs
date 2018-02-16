namespace TamTam.Trailers.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using TamTam.Trailers.Web.Filters;
    using TamTam.Trailers.Web.Model.Movie;
    using TamTam.Trailers.Web.Services.Movies;
    using TamTam.Trailers.Web.Services.Movies.Model;

    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly IMovieService service;

        public MoviesController(IMovieService service)
        {
            this.service = service;
        }

        /// <summary>
        ///     Searches trailers
        /// </summary>
        /// <returns></returns>
        [ValidateModelState]
        [HttpGet("[action]")]
        public async Task<IEnumerable<Movie>> Search(SearchViewModel model)
        {
            var movies = await service.Search(model.Query);
            return movies.Where(x => !string.IsNullOrWhiteSpace(x.Poster));
        }

        /// <summary>
        ///     Searches trailers
        /// </summary>
        /// <returns></returns>
        [ValidateModelState]
        [HttpGet("{id}")]
        public async Task<Movie> Get(string id)
        {
            return await service.Get(id);
        }
    }
}
namespace TamTam.Trailers.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using TamTam.Trailers.Web.Filters;
    using TamTam.Trailers.Web.Model;
    using TamTam.Trailers.Web.Services.Movies;

    [ValidateModelState]
    [Route("api/[controller]")]
    [ResponseCache(Duration = 60 * 5)]
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
        [HttpGet("[action]")]
        public async Task<IEnumerable<Movie>> Search(string query)
        {
            var movies = await service.Search(query);
            return movies.Where(x => !string.IsNullOrWhiteSpace(x.Poster));
        }

        /// <summary>
        ///     Searches trailers
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Movie> Get(string id)
        {
            return await service.Get(id);
        }
    }
}
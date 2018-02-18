namespace TamTam.Trailers.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TamTam.Trailers.Infrastructure.Filters;
    using TamTam.Trailers.Infrastructure.Model;
    using TamTam.Trailers.Infrastructure.Services;

    [ValidateModelState]
    [Route("api/[controller]")]
    [ResponseCache(Duration = 60 * 5)]
    public class MoviesController : Controller
    {
        #region Fields

        private readonly IMovieService service;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MoviesController"/> class.
        /// </summary>
        /// <param name="service">The movie service.</param>
        public MoviesController(IMovieService service)
        {
            this.service = service;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a movie with the speficied identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A movie with teh speficied identifier</returns>
        [HttpGet("{id}")]
        public async Task<Movie> Get(string id)
        {
            return await service.Get(id);
        }

        /// <summary>
        /// Searches movies matching the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A collection of movies</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<Movie>> Search(string query)
        {
            var movies = await service.Search(query);
            // Exclude movies with no poster (just because they look bad)
            return movies.Where(x => !string.IsNullOrWhiteSpace(x.Poster));
        }

        #endregion
    }
}
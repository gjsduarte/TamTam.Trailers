namespace TamTam.Trailers.Web.Services.Movies
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TamTam.Trailers.Web.Model;

    public interface IMovieService
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets a movie.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A movie with teh speficied identifier</returns>
        Task<Movie> Get(string id);

        /// <summary>
        /// Searches movies matching the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A collection of movies</returns>
        Task<IEnumerable<Movie>> Search(string query);

        #endregion
    }
}
namespace TamTam.Trailers.Web.Services.Movies
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TamTam.Trailers.Web.Services.Movies.Model;

    public interface IMovieService
    {
        Task<IEnumerable<Movie>> Search(string query);
        
        Task<Movie> Get(string id);
    }
}
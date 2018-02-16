namespace TamTam.Trailers.Web.Model.Movie
{
    using System.ComponentModel.DataAnnotations;

    public class SearchViewModel
    {
        [Required]
        public string Query { get; set; }
    }
}
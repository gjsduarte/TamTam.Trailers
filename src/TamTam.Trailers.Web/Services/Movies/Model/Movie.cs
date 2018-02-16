namespace TamTam.Trailers.Web.Services.Movies.Model
{
    public class Movie
    {
        public string Id { get; set; }
        
        public double? Rating { get; set; }
        
        public string Title { get; set; }
        
        public string Poster { get; set; }
        
        public int? Year { get; set; }
        
        public string Plot { get; set; }
    }
}
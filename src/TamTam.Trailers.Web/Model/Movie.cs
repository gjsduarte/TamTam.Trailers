namespace TamTam.Trailers.Web.Model
{
    public class Movie
    {
        public string Id { get; set; }

        public double? Rating { get; set; }

        public string Title { get; set; }

        public string Poster { get; set; }

        public int? Year { get; set; }

        public string Plot { get; set; }

        public string Backdrop { get; set; }

        public string ImdbId { get; set; }
    }
}
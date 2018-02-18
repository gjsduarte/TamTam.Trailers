namespace TamTam.Trailers.Infrastructure.Model
{
    public class Movie
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the movie backdrop image path.
        /// </summary>
        public string Backdrop { get; set; }

        /// <summary>
        /// Gets or sets the movie identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the movie imdb identifier.
        /// </summary>
        public string ImdbId { get; set; }

        /// <summary>
        /// Gets or sets the movie plot.
        /// </summary>
        public string Plot { get; set; }

        /// <summary>
        /// Gets or sets the movie poster image path.
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// Gets or sets the movie imdb rating.
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        /// Gets or sets the movie title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the movie release year.
        /// </summary>
        public int? Year { get; set; }

        #endregion
    }
}
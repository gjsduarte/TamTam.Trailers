namespace TamTam.Trailers.Infrastructure.Model
{
    public class Video
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the video service key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the video name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the video service type.
        /// </summary>
        public VideoType Type { get; set; }

        #endregion
    }
}
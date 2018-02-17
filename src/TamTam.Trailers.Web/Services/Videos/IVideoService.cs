namespace TamTam.Trailers.Web.Services.Videos
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TamTam.Trailers.Web.Model;

    public interface IVideoService
    {
        #region Public Methods and Operators

        /// <summary>
        /// Searches videos for the specified movie identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A colelction of videos</returns>
        Task<IEnumerable<Video>> Search(string id);

        #endregion
    }
}
namespace TamTam.Trailers.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TamTam.Trailers.Infrastructure.Extensions;
    using TamTam.Trailers.Infrastructure.Filters;
    using TamTam.Trailers.Infrastructure.Model;
    using TamTam.Trailers.Infrastructure.Services;

    [ValidateModelState]
    [Route("api/[controller]")]
    [ResponseCache(Duration = 60 * 5)]
    public class VideosController : Controller
    {
        #region Fields

        private readonly IEnumerable<IVideoService> services;

        #endregion

        #region Constructors and Destructors

        public VideosController(IEnumerable<IVideoService> services)
        {
            this.services = services;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets videos for the specified movie identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A colelction of videos</returns>
        [HttpGet("{id}")]
        public async Task<IEnumerable<Video>> Get(string id)
        {
            // Fetch videos from all registered services
            var videos = new List<Video>();
            var tasks = services.Select(async service =>
            {
                var results = await service.Search(id);
                // Ups! Preventing threding issues here! ;)
                lock (videos)
                {
                    videos.AddRange(results);
                }
            });

            await Task.WhenAll(tasks);

            // Aggregate videos so we don't return duplicates
            return videos.DistinctBy(x => x.Key);
        }

        #endregion
    }
}
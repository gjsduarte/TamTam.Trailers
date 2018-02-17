namespace TamTam.Trailers.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Query.Internal;
    using TamTam.Trailers.Web.Extensions;
    using TamTam.Trailers.Web.Filters;
    using TamTam.Trailers.Web.Model;
    using TamTam.Trailers.Web.Services.Videos;

    [ValidateModelState]
    [Route("api/[controller]")]
    [ResponseCache(Duration = 60 * 5)]
    public class VideosController : Controller
    {
        private readonly IEnumerable<IVideoService> services;

        public VideosController(IEnumerable<IVideoService> services)
        {
            this.services = services;
        }

        /// <summary>
        ///     Searches trailers
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IEnumerable<Video>> Get(string id)
        {
            var videos = new List<Video>();
            var tasks = services.Select(
                async service =>
                {
                    var results = await service.Search(id);
                    lock (videos)
                    {
                        videos.AddRange(results);
                    }
                });
            await Task.WhenAll(tasks);
            // Aggregate videos so we don't return duplicates
            return videos.DistinctBy(x => x.Key);
        }
    }
}
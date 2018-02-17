namespace TamTam.Trailers.Web.Services.Videos
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TamTam.Trailers.Web.Model;

    public interface IVideoService
    {
        Task<IEnumerable<Video>> Search(string id);
    }
}
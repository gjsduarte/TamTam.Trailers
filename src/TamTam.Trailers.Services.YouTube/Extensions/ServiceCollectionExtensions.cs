namespace TamTam.Trailers.Services.YouTube.Extensions
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using TamTam.Trailers.Infrastructure.Services;
    using TamTam.Trailers.Services.YouTube.Options;

    public static class ServiceCollectionExtensions
    {
        #region Public Methods and Operators

        public static IServiceCollection AddYouTube(this IServiceCollection services, Action<YoutubeOptions> configure)
        {
            services.AddScoped<IVideoService, YoutubeVideoService>();
            services.Configure(configure);
            return services;
        }

        #endregion
    }
}
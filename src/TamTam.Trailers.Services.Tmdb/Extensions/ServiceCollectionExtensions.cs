namespace TamTam.Trailers.Services.Tmdb.Extensions
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using TamTam.Trailers.Infrastructure.Services;
    using TamTam.Trailers.Services.Tmdb.Options;

    public static class ServiceCollectionExtensions
    {
        #region Public Methods and Operators

        public static IServiceCollection AddTmdb(this IServiceCollection services, Action<TmdbOptions> configure)
        {
            services.AddScoped<IMovieService, TmdbMovieService>();
            services.AddScoped<IVideoService, TmdbVideoService>();
            services.Configure(configure);
            return services;
        }

        #endregion
    }
}
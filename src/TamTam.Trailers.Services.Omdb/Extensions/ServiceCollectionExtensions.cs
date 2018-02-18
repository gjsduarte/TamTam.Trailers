namespace TamTam.Trailers.Services.Omdb.Extensions
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using TamTam.Trailers.Infrastructure.Services;
    using TamTam.Trailers.Services.Omdb.Options;

    public static class ServiceCollectionExtensions
    {
        #region Public Methods and Operators

        public static IServiceCollection AddOmdb(this IServiceCollection services, Action<OmdbOptions> configure)
        {
            services.AddScoped<IMovieService, OmdbMovieService>();
            services.Configure(configure);
            return services;
        }

        #endregion
    }
}
namespace TamTam.Trailers.Web
{
    using Microsoft.Extensions.DependencyInjection;
    using TamTam.Trailers.Web.Factories;
    using TamTam.Trailers.Web.Services.Movies;
    using TamTam.Trailers.Web.Services.Videos;

    internal static partial class StartupExtensions
    {
        internal static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            // services.AddScoped<IMovieService, OmdbMovieService>();
            services.AddScoped<IMovieService, TmdbMovieService>();
            services.AddScoped<IVideoService, TmdbVideoService>();
            services.AddScoped<IVideoService, YoutubeVideoService>();

            services.AddScoped<IHttpClientFactory, HttpClientFactory>();

            return services;
        }
    }
}
namespace TamTam.Trailers.Web
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TamTam.Trailers.Infrastructure.Factories;
    using TamTam.Trailers.Services.Tmdb.Extensions;
    using TamTam.Trailers.Services.YouTube.Extensions;

    internal static partial class StartupExtensions
    {
        internal static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // services.AddOmdb(options => configuration.Bind("Omdb", options));
            services.AddTmdb(options => configuration.Bind("Tmdb", options));
            services.AddYouTube(options => configuration.Bind("YouTube", options));
            
            // Add services
            services.AddScoped<IHttpClientFactory, HttpClientFactory>();
            
            return services;
        }
    }
}
namespace TamTam.Trailers.Web
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using TamTam.Trailers.Web.Options;

    internal static partial class StartupExtensions
    {
        #region Methods

        /// <summary>
        ///     Configures the settings by binding the contents of the appsetings.json file to the specified Plain Old CLR
        ///     Objects (POCO) and adding <see cref="IOptions{TOptions}" /> objects to the services collection.
        /// </summary>
        /// <param name="services">The services collection or IoC container.</param>
        /// <param name="configuration">
        ///     Gets or sets the application configuration, where key value pair settings are
        ///     stored.
        /// </param>
        internal static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OmdbOptions>(configuration.GetSection("Omdb"));
            services.Configure<TmdbOptions>(configuration.GetSection("Tmdb"));
            services.Configure<YoutubeOptions>(configuration.GetSection("Youtube"));

            return services;
        }

        #endregion
    }
}
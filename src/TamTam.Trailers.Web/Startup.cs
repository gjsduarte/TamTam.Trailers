namespace TamTam.Trailers.Web
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Startup
    {
        #region Constructors and Destructors

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            Logger = loggerFactory.CreateLogger<Startup>();
        }

        #endregion

        #region Public Properties

        private IConfiguration Configuration { get; }

        private ILogger<Startup> Logger { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Configures the application.
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseErrorPages(env);
            app.UseStaticFiles();
            app.UseApiDocs();
            app.UseSpaStaticFiles();
            app.UseMvc();
            app.UseSpa(env);
        }

        /// <summary>
        /// Configures the application services.
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddApiDocs(Logger);
            services.AddSpa();
            services.AddServices(Configuration);
        }

        #endregion
    }
}
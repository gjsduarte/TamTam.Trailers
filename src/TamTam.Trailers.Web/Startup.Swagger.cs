namespace TamTam.Trailers.Web
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    internal static partial class StartupExtensions
    {
        #region Methods

        internal static IServiceCollection AddApiDocs(this IServiceCollection services, ILogger logger)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = GetTitle(), 
                    Version = $"v1{GetVersion()}"
                });

                // integrate xml comments
                options.IncludeXmlDocumentation(logger);
            });

            return services;
        }

        internal static IApplicationBuilder UseApiDocs(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{GetTitle()} v{GetVersion()}");
            });

            return app;
        }

        private static string GetTitle()
        {
            return typeof(StartupExtensions).Namespace;
        }

        private static string GetVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.ProductVersion;
        }

        private static SwaggerGenOptions IncludeXmlDocumentation(this SwaggerGenOptions options, ILogger logger)
        {
            var basePath = AppContext.BaseDirectory;
            foreach (var file in Directory.EnumerateFiles(basePath, "*.xml"))
            {
                try
                {
                    options.IncludeXmlComments(file);
                }
                catch (Exception exception)
                {
                    logger.LogWarning(exception, $"XML file {file} contains no valid documentation information.");
                }
            }

            return options;
        }

        #endregion
    }
}
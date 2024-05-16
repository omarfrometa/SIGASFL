using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace SIGASFL.Restful.Extensions
{
    public static class HelpExtension
    {
        private static string _ApiHelpTitle;
        private static string _ApiHelpDescription;
        /// <summary>
        /// Register the Swagger generator, defining 1 or more Swagger documents
        /// </summary>
        /// <param name="services"></param>
        public static void AddApiHelp(this IServiceCollection services)
        {
            var assemblyObj = Assembly.GetEntryAssembly();
            var assemblyExecuting = Assembly.GetExecutingAssembly();
            _ApiHelpTitle = assemblyObj.GetName().Name;
            var assemblyDesc = assemblyObj
                .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                .OfType<AssemblyDescriptionAttribute>()
                .FirstOrDefault();

            _ApiHelpDescription = ((assemblyDesc == null) ? _ApiHelpTitle : assemblyDesc.Description) + $" ({assemblyObj.GetName().Version})";
            //_ApiHelpDescription = $"{_ApiHelpDescription}<br>{assemblyObj.GetCreationTime().ToString("yyyy.MM.dd hh:mm:ss tt")}";

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = _ApiHelpTitle,
                        Version = "v1",
                        Description = _ApiHelpDescription,
                    });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{assemblyObj.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);

                var sharedXmlFile = $"{assemblyExecuting.GetName().Name}.xml";
                var sharedXmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sharedXmlFile);

                if (File.Exists(sharedXmlPath))
                    c.IncludeXmlComments(sharedXmlPath);

                var modelXmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SIGASFL.Models.xml");

                if (File.Exists(modelXmlPath))
                    c.IncludeXmlComments(modelXmlPath);

                //Add Authorization header
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                //Example to add header to specific API:
                //if (!string.IsNullOrEmpty(assemblyObj.FullName) && assemblyObj.FullName.Contains(""))
                //    c.OperationFilter<AddHeaderParameterFilter>();
            });
        }

        /// <summary>
        /// Enable middleware to serve generated Swagger as a JSON endpoint.
        /// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
        /// specifying the Swagger JSON endpoint.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        public static void UseApiHelp(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (configuration.GetSection("HelpSettings").GetValue<bool>("IsEnable"))
            {
                app.UseSwagger(o =>
                {
                    o.RouteTemplate = "help/{documentName}/help.json";
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/help.json", _ApiHelpDescription + " v1");
                    c.DocumentTitle = _ApiHelpDescription;
                    c.RoutePrefix = "help";
                });
            }
        }
    }
}

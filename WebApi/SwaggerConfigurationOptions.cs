using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Sparkle.WebApi
{
    public class SwaggerConfigurationOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            string webAtiXmlName = Assembly.GetExecutingAssembly().GetName().Name ?? "api";
            string webApiXmlPath = Path.Combine(AppContext.BaseDirectory, webAtiXmlName + ".xml");
            options.IncludeXmlComments(webApiXmlPath);

            string appXmlName = "Application";
            string appXmlPath = Path.Combine(AppContext.BaseDirectory, appXmlName + ".xml");
            options.IncludeXmlComments(appXmlPath);

            options.SwaggerDoc("sparkle", new OpenApiInfo
            {
                Title = "Spark API",
                Contact = new OpenApiContact
                {
                    Email = "dneshotkin@gmail.com",
                    Name = "Sparkle",
                    Url = new("https://github.com/SparkChats")
                },
                Description = "Api for itstep graduate work",
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Name = "Authorization",
                BearerFormat = "JWT",
                Scheme = "bearer",
                Description = "Enter your JWT token"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                       },
                    Array.Empty<string>()
                }
            });

            options.CustomOperationIds(apiDescription =>
            apiDescription.TryGetMethodInfo(out MethodInfo? method)
            ? method.Name : null);
        }
    }
}

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WebApi
{
    public class SwaggerConfigurationOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            string name = Assembly.GetExecutingAssembly().GetName().Name ?? "api";
            string path = Path.Combine(AppContext.BaseDirectory, name + ".xml");
            options.IncludeXmlComments(path);

            options.SwaggerDoc("spark", new OpenApiInfo
            {
                Title = "Spark API",
                Contact = new OpenApiContact
                {
                    Email = "dneshotkin@gmail.com",
                    Name = "Spark",
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

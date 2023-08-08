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

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Discord API",
                Contact = new OpenApiContact
                {
                    Email = "dneshotkin@gmail.com",
                    Name = "NewDiscordClone",
                    Url = new("https://github.com/NewDiscordClone")
                },
                Version = "v1",
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

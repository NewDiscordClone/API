using Application;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace WebApi
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            IServiceCollection services = builder.Services;

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use camelCase property names in JSON
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                // Other JSON formatting options can be configured here
            });

            services.AddApplication();

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:7198";
                    options.Audience = "MessageApi";
                    options.RequireHttpsMetadata = false;
                });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddSwaggerGen(options =>
            {
                string xmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlName);
                options.IncludeXmlComments(xmlPath);
            });

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapSwagger();
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
                    option.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.MapControllers();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}
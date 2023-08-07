using Application;
using Application.Hubs;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Application
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            IServiceCollection services = builder.Services;

            services.AddApplication();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use camelCase property names in JSON
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                // Other JSON formatting options can be configured here
            });
            services.AddDatabase(builder.Configuration);

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

            builder.Services.AddSignalR();
            //builder.Services.AddSingleton<IChatService, ChatService>();

            WebApplication app = builder.Build();

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<ChatHub>("chat");

            if (app.Environment.IsDevelopment())
            {
                app.MapSwagger();
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
                    option.RoutePrefix = string.Empty;
                });
            }

            app.Run();
        }
    }
}
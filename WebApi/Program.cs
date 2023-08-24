using Application;
using Application.Common.Mapping;
using Application.Hubs;
using Application.Interfaces;
using Application.Providers;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using WebApi.Attributes;
using WebApi.Authorization;
using WebApi.Providers;

namespace WebApi
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
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            });
            services.AddDatabase(builder.Configuration);

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(
                    Assembly.GetExecutingAssembly()));
                config.AddProfile(new AssemblyMappingProfile(typeof(IAppDbContext).Assembly));
            });

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

            services.AddAuthorization();

            services.AddScoped<IAuthorizedUserProvider, AuthorizedUserProvider>();
            services.AddSingleton<IAuthorizationPolicyProvider, ServerAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, ServerMemberAuthorizationHandler>();
            services.AddScoped<IActionFilter, ServerAuthorizeAttribute>();

            services.AddHttpContextAccessor();

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

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
                 SwaggerConfigurationOptions>();
            services.AddSwaggerGen();

            builder.Services.AddSignalR();

            WebApplication app = builder.Build();

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<ChatHub>("chat");
            app.MapSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/spark/swagger.json", "WebApi");
                option.RoutePrefix = string.Empty;
                option.DisplayRequestDuration();
            });
            if (app.Environment.IsDevelopment())
            {

            }

            app.Run();
        }
    }
}
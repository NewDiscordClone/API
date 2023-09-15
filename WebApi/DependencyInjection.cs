using Microsoft.Extensions.Options;
using Sparkle.WebApi.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sparkle.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilterAttribute>();
            });

            services.AddSwagger();

            services.AddSignalR();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<
                SwaggerGenOptions>,
                SwaggerConfigurationOptions>();

            services.AddSwaggerGen();

            return services;
        }
    }
}

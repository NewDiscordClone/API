using Microsoft.Extensions.Options;
using Sparkle.Application.Common.Convertors;
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
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new PrivateChatLookUpConverter());
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

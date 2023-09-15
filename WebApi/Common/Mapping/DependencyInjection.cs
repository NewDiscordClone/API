using Mapster;
using MapsterMapper;
using System.Reflection;

namespace Sparkle.WebApi.Common.Mapping
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}

namespace Sparkle.WebApi.Common.Options
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.Configure<TenorOptions>(configuration.GetSection(TenorOptions.SectionName));

            return services;
        }
    }
}

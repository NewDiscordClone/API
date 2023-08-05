using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AuthorizationDbContext>(options
                => options.UseSqlServer(configuration
                .GetConnectionString("AuthorizationDbContextConnection")));

            services.AddScoped<AuthorizationDbContext>();

            return services;
        }
    }
}

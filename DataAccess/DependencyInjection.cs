using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options
                =>
            {
                options.UseSqlServer(configuration
                    .GetConnectionString("SqlServer") ?? throw new Exception("Connection doesn't exist"));
            });

            services.AddScoped<IAppDbContext, AppDbContext>();

            return services;
        }
    }
}

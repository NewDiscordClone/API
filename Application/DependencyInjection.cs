using Microsoft.Extensions.DependencyInjection;
using Sparkle.Application.Common.Factories;
using Sparkle.Application.Common.Interfaces;
using System.Reflection;

namespace Sparkle.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(
                Assembly.GetExecutingAssembly()));

            services.AddScoped<IRoleFactory, RoleFactory>();
            return services;
        }
    }
}

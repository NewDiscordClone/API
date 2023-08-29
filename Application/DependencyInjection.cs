using Application.Common.Factories;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
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

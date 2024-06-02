using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sparkle.Application.Common.Behaviors;
using Sparkle.Application.Common.Convertor;
using Sparkle.Application.Common.Factories;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Mapping;
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

            services.AddScoped(
              typeof(IPipelineBehavior<,>),
              typeof(ValidationBehavior<,>));
            services.AddTransient(
              typeof(IPipelineBehavior<,>),
              typeof(LoggingBehavior<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(typeof(Domain.LookUps.MessageDto).Assembly));
                config.AddProfile(new AssemblyMappingProfile(typeof(IAppDbContext).Assembly));
            });

            services.AddScoped<IConvertor, Convertor>();

            services.AddScoped<IRoleFactory, RoleFactory>();
            services.AddScoped<ServerFactory>();

            return services;
        }
    }
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sparkle.Application.Common.Behaviors;
using Sparkle.Application.Common.Factories;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Mapping;
using Sparkle.Application.Users.Relationships.Common;
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
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(
                    Assembly.GetExecutingAssembly()));
                config.AddProfile(new AssemblyMappingProfile(typeof(IAppDbContext).Assembly));
            });

            services.AddScoped<IRoleFactory, RoleFactory>();
            services.AddScoped<IRelationshipConvertor, RelationshipConvertor>();

            return services;
        }
    }
}

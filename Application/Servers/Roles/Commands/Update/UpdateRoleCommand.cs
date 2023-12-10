using MediatR;
using Microsoft.AspNetCore.Identity;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.Roles.Commands.Update
{
    public record UpdateRoleCommand : IRequest<Role>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Color { get; init; }
        public int Priority { get; init; }
        public IEnumerable<IdentityRoleClaim<Guid>> Claims { get; init; }

    }
}

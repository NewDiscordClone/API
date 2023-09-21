using MediatR;
using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Commands.Update
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

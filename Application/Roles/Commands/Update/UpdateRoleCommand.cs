using MediatR;
using Sparkle.Application.Models;
using System.Security.Claims;

namespace Sparkle.Application.Roles.Commands.Update
{
    public record UpdateRoleCommand : IRequest<Role>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Color { get; init; }
        public int Priority { get; init; }
        public IEnumerable<Claim> Claims { get; init; }

    }
}

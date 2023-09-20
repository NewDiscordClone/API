using MediatR;
using Sparkle.Application.Models;
using System.Security.Claims;

namespace Sparkle.Application.Roles.Commands.UpdateClaims
{
    public record UpdateRoleClaimsCommand : IRequest<Role>
    {
        public Guid RoleId { get; init; }
        public IEnumerable<Claim> Claims { get; init; }
    }
}

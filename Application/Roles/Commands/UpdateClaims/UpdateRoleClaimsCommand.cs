using MediatR;
using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Commands.UpdateClaims
{
    public record UpdateRoleClaimsCommand : IRequest<Role>
    {
        public Guid RoleId { get; init; }
        public IEnumerable<IdentityRoleClaim<Guid>> Claims { get; init; }
    }
}

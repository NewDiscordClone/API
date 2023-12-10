using MediatR;
using Microsoft.AspNetCore.Identity;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.Roles.Queries.RoleDetails
{
    public record RoleDetailsQuery(Guid RoleId) : IRequest<(Role, List<IdentityRoleClaim<Guid>>)>
    {
    }
}

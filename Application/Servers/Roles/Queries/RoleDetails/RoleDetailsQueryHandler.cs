using MediatR;
using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Queries.RoleDetails
{
    public class RoleDetailsQueryHandler(IRoleRepository roleRepository)
        : IRequestHandler<RoleDetailsQuery, (Role, List<IdentityRoleClaim<Guid>>)>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<(Role, List<IdentityRoleClaim<Guid>>)> Handle(RoleDetailsQuery query, CancellationToken cancellationToken)
        {
            Role role = await _roleRepository.FindAsync(query.RoleId, cancellationToken);
            List<IdentityRoleClaim<Guid>> claims = await _roleRepository.GetRoleClaimAsync(role, cancellationToken);
            return (role, claims);
        }
    }
}

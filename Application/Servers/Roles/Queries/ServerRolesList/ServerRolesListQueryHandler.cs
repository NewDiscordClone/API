using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Queries.ServerRolesList
{
    public class ServerRolesListQueryHandler(IRoleRepository roleRepository)
        : IRequestHandler<ServerRolesListQuery, List<Role>>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<List<Role>> Handle(ServerRolesListQuery query, CancellationToken cancellationToken)
        {
            List<Role> roles = await _roleRepository.ExecuteCustomQuery(roles => roles
            .Where(role => role.ServerId == query.ServerId))
            .ToListAsync(cancellationToken);

            return roles;
        }
    }
}

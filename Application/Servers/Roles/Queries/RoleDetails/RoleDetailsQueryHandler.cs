using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Queries.RoleDetails
{
    public class RoleDetailsQueryHandler : IRequestHandler<RoleDetailsQuery, Role>
    {
        private readonly IRoleRepository _roleRepository;
        public RoleDetailsQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> Handle(RoleDetailsQuery query, CancellationToken cancellationToken)
        {
            Role role = await _roleRepository.FindAsync(query.RoleId, cancellationToken);
            return role;
        }
    }
}

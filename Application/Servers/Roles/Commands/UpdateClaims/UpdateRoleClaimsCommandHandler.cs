using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.UpdateClaims
{
    public class UpdateRoleClaimsCommandHandler : IRequestHandler<UpdateRoleClaimsCommand, Role>
    {
        private readonly IRoleRepository _roleRepository;
        public UpdateRoleClaimsCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> Handle(UpdateRoleClaimsCommand command, CancellationToken cancellationToken)
        {
            Role role = await _roleRepository.FindAsync(command.RoleId, cancellationToken);

            await _roleRepository.RemoveClaimsFromRoleAsync(role, cancellationToken);
            await _roleRepository.AddClaimsToRoleAsync(role, command.Claims, cancellationToken);

            return role;
        }
    }
}

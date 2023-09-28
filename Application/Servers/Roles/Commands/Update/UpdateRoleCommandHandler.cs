using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.Update
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Role>
    {
        private readonly IRoleRepository _roleRepository;
        public UpdateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            Role role = await _roleRepository.FindAsync(command.Id, cancellationToken);

            if (_roleRepository.IsPriorityUniqueInServer(role.ServerId!, role.Priority))
                throw new InvalidOperationException("Priority must be unique in server");

            await _roleRepository.RemoveClaimsFromRoleAsync(role, cancellationToken);

            await _roleRepository.AddClaimsToRoleAsync(role, command.Claims, cancellationToken);
            role.Name = command.Name;
            role.Color = command.Color;
            role.Priority = command.Priority;

            await _roleRepository.UpdateAsync(role, cancellationToken);

            return role;
        }
    }
}

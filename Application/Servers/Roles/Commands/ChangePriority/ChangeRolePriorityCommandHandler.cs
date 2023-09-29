using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.ChangePriority
{
    public class ChangeRolePriorityCommandHandler : IRequestHandler<ChangeRolePriorityCommand, Role>
    {
        private readonly IRoleRepository _roleRepository;
        public ChangeRolePriorityCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> Handle(ChangeRolePriorityCommand command, CancellationToken cancellationToken)
        {
            Role role = await _roleRepository.FindAsync(command.RoleId, cancellationToken);

            if (_roleRepository.IsPriorityUniqueInServer(role.ServerId!, role.Priority))
                throw new InvalidOperationException("Priority must be unique in server");

            role.Priority = command.Priority;

            await _roleRepository.UpdateAsync(role, cancellationToken);
            return role;
        }
    }
}

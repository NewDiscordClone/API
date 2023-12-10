using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeRangePriority
{
    public class ChangeRangePriorityCommandHandler : IRequestHandler<ChangeRangePriorityCommand, IEnumerable<Role>>
    {
        private readonly IRoleRepository _roleRepository;

        public ChangeRangePriorityCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<Role>> Handle(ChangeRangePriorityCommand command, CancellationToken cancellationToken)
        {
            List<Role> roles = await _roleRepository.FilterAsync(role =>
                command.Priorities.Keys.Contains(role.Id), cancellationToken);

            foreach (Role role in roles)
            {
                role.Priority = command.Priorities[role.Id];
                await _roleRepository.UpdateAsync(role, cancellationToken);
            }

            return roles;
        }
    }
}

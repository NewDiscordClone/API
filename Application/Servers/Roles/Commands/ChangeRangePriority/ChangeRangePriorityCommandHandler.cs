using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeRangePriority
{
    public class ChangeRangePriorityCommandHandler(IRoleRepository roleRepository)
        : IRequestHandler<ChangeRangePriorityCommand, IEnumerable<Role>>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<IEnumerable<Role>> Handle(ChangeRangePriorityCommand command, CancellationToken cancellationToken)
        {
            List<Role> roles = await _roleRepository.ExecuteCustomQuery(roles => roles
                .Where(role => command.Priorities.Keys.Contains(role.Id)))
                .ToListAsync(cancellationToken);

            foreach (Role role in roles)
            {
                role.Priority = command.Priorities[role.Id];
                await _roleRepository.UpdateAsync(role, cancellationToken);
            }

            return roles;
        }
    }
}

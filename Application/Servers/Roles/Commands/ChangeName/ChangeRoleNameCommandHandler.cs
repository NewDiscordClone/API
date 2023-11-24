using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeName
{
    public class ChangeRoleNameCommandHandler(IRoleRepository repository) : IRequestHandler<ChangeRoleNameCommand, Role>
    {
        private readonly IRoleRepository _repository = repository;

        public async Task<Role> Handle(ChangeRoleNameCommand command, CancellationToken cancellationToken)
        {
            Role role = await _repository.FindAsync(command.RoleId, cancellationToken);
            role.Name = command.Name;

            await _repository.UpdateAsync(role, cancellationToken);
            return role;
        }
    }
}

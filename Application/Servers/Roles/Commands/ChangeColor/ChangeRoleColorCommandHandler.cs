using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeColor
{
    public class ChangeRoleColorCommandHandler(IRoleRepository repository) : IRequestHandler<ChangeRoleColorCommand, Role>
    {
        private readonly IRoleRepository _repository = repository;

        public async Task<Role> Handle(ChangeRoleColorCommand command, CancellationToken cancellationToken)
        {
            Role role = await _repository.FindAsync(command.RoleId, cancellationToken);
            role.Color = command.Color;

            await _repository.UpdateAsync(role, cancellationToken);
            return role;
        }
    }
}

using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.Delete
{
    public class DeleteRoleCommandHandler(IServerProfileRepository serverProfileRepository,
        IServerRepository serverRepository,
        IRoleRepository roleRepository) : IRequestHandler<DeleteRoleCommand, Role>
    {
        private readonly IServerProfileRepository _serverProfileRepository = serverProfileRepository;
        private readonly IServerRepository _serverRepository = serverRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<Role> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            Role role = await _roleRepository.FindAsync(command.RoleId, cancellationToken);

            if (role.ServerId is null)
                throw new InvalidOperationException("Role is not associated with a server");

            Server server = await _serverRepository.FindAsync(role.ServerId, cancellationToken);

            server.Roles.Remove(role.Id);
            await _serverProfileRepository.RemoveRoleFromServerProfilesAsync(role, server.Id, cancellationToken);
            await _roleRepository.DeleteAsync(role, cancellationToken);

            await _serverRepository.UpdateAsync(server, cancellationToken);

            return role;
        }
    }
}

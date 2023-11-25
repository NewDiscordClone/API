using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.Create
{
    public class CreateRoleCommandHandler(IMapper mapper,
        IRoleFactory roleFactory,
        IServerRepository serverRepository,
        IRoleRepository roleRepository) : RequestHandler(mapper), IRequestHandler<CreateRoleCommand, Role>
    {
        private readonly IRoleFactory _roleFactory = roleFactory;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IServerRepository _serverRepository = serverRepository;

        public async Task<Role> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            Server server = await _serverRepository.FindAsync(command.ServerId, cancellationToken);
            Role role = Mapper.Map<Role>(command);

            await _roleFactory.CreateServerRoleAsync(role, command.Claims);

            server.Roles.Add(role.Id);
            await _serverRepository.UpdateAsync(server, cancellationToken);
            return role;
        }
    }
}

using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.Roles.Commands.Create
{
    public class CreateRoleCommandHandler : RequestHandlerBase, IRequestHandler<CreateRoleCommand, Role>
    {
        private readonly IRoleFactory _roleFactory;
        private readonly IRoleRepository _roleRepository;
        public CreateRoleCommandHandler(IAppDbContext context, IMapper mapper, IRoleFactory roleFactory, IRoleRepository roleRepository) : base(context, mapper)
        {
            _roleFactory = roleFactory;
            _roleRepository = roleRepository;
        }

        public async Task<Role> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(command.ServerId, cancellationToken);

            Role role = Mapper.Map<Role>(command);

            await _roleFactory.CreateServerRoleAsync(role, command.Claims);

            server.Roles.Add(role.Id);
            await Context.Servers.UpdateAsync(server, cancellationToken);
            return role;
        }
    }
}

using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.CreateServer
{
    public class CreateServerCommandHandler : RequestHandlerBase, IRequestHandler<CreateServerCommand, Server>
    {
        private readonly IRoleFactory _roleFactory;
        private readonly IServerProfileRepository _serverProfileRepository;
        public async Task<Server> Handle(CreateServerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = new()
            {
                Title = command.Title,
                Image = command.Image,
            };

            List<Role> roles = _roleFactory.GetDefaultServerRoles();

            Role ownerRole = roles.First(role => role.Name == Constants.Roles.ServerOwnerName);

            ServerProfile owner = new()
            {
                UserId = UserId,
                ServerId = server.Id,
                Roles = { ownerRole }
            };

            server.Roles.AddRange(roles.ConvertAll(role => role.Id));
            server.Profiles.Add(owner.Id);

            await _serverProfileRepository.AddAsync(owner, cancellationToken);
            await Context.Servers.AddAsync(server, cancellationToken);

            return server;
        }

        public CreateServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IRoleFactory roleFactory, IServerProfileRepository serverProfileRepository)
            : base(context, userProvider)
        {
            _roleFactory = roleFactory;
            _serverProfileRepository = serverProfileRepository;
        }
    }
}
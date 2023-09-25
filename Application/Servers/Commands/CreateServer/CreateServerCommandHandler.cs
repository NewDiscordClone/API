using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.CreateServer
{
    public class CreateServerCommandHandler : RequestHandlerBase, IRequestHandler<CreateServerCommand, Server>
    {
        private readonly IRoleFactory _roleFactory;
        public async Task<Server> Handle(CreateServerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);


            //TODO: Добавить роли
            Server server = new()
            {
                Title = command.Title,
                Image = command.Image,
            };

            List<Role> roles = _roleFactory.GetDefaultServerRoles(server.Id);


            Role ownerRole = roles.First(role => role.Name == Constants.ServerProfile.DefaultOwnerRoleName);

            ServerProfile owner = new()
            {
                UserId = UserId,
                ServerId = server.Id,
                Roles = { ownerRole }
            };

            server.Roles.AddRange(roles.ConvertAll(role => role.Id));
            server.Profiles.Add(owner.Id);

            await Context.UserProfiles.AddAsync(owner, cancellationToken);
            await Context.Servers.AddAsync(server);

            return server;
        }

        public CreateServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IRoleFactory roleFactory)
            : base(context, userProvider)
        {
            _roleFactory = roleFactory;
        }
    }
}
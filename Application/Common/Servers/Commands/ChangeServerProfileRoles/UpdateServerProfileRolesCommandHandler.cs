using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Servers.Commands.ChangeServerProfileRoles
{
    public class UpdateServerProfileRolesCommandHandler : RequestHandlerBase, IRequestHandler<UpdateServerProfileRolesCommand>
    {
        public UpdateServerProfileRolesCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task Handle(UpdateServerProfileRolesCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(command.ServerId);

            if (server.Owner != UserId) //TODO: Замінити на перевірку claims
                throw new NoPermissionsException("You don't have an appropriate right to do this");

            ServerProfile serverProfile = server.ServerProfiles.Find(sp => sp.UserId == command.UserId)
                ?? throw new NoPermissionsException("The user are not a member of the server");

            serverProfile.Roles = new List<Role>();
            foreach (Guid roleId in command.Roles)
            {
                serverProfile.Roles.Add(await Context.SqlRoles.FindAsync(roleId));
            }

            await Context.Servers.UpdateAsync(server);
        }
    }
}
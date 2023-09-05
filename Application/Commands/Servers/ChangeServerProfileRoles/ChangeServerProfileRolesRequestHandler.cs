using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Servers.ChangeServerProfileRoles
{
    public class ChangeServerProfileRolesRequestHandler : RequestHandlerBase, IRequestHandler<ChangeServerProfileRolesRequest>
    {
        public ChangeServerProfileRolesRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task Handle(ChangeServerProfileRolesRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(request.ServerId);
            if (server.Owner != UserId) //TODO: Замінити на перевірку claims
                throw new NoPermissionsException("You don't have an appropriate right to do this");
            ServerProfile serverProfile = server.ServerProfiles.Find(sp => sp.UserId == request.UserId) ??
                                          throw new NoPermissionsException("The user are not a member of the server");
            serverProfile.Roles = new List<Role>();
            foreach (var roleId in request.Roles)
                serverProfile.Roles.Add(await Context.SqlRoles.FindAsync(roleId));
            await Context.Servers.UpdateAsync(server);
        }
    }
}
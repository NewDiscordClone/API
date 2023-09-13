using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Common.Servers.Commands.KickUser
{
    public class KickUserRequestHandler : RequestHandlerBase, IRequestHandler<KickUserRequest>
    {
        public KickUserRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task Handle(KickUserRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(request.ServerId);

            if (server.Owner != UserId) //TODO: Замінити на логіку перевірки claim
                throw new NoPermissionsException("You don't have the appropriate rights");

            ServerProfile userToRemove = server.ServerProfiles.Find(sp => sp.UserId == request.UserId) ??
                                throw new Exception("The user are not a member of the server");
            server.ServerProfiles.Remove(userToRemove);

            await Context.Servers.UpdateAsync(server);
        }
    }
}
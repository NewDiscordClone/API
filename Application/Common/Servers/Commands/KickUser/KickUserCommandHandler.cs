using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Servers.Commands.KickUser
{
    public class KickUserCommandHandler : RequestHandlerBase, IRequestHandler<KickUserCommand>
    {
        public KickUserCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task Handle(KickUserCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(command.ServerId);

            if (server.Owner != UserId) //TODO: Замінити на логіку перевірки claim
                throw new NoPermissionsException("You don't have the appropriate rights");

            ServerProfile userToRemove = server.ServerProfiles.Find(sp => sp.UserId == command.UserId) ??
                                throw new Exception("The user are not a member of the server");
            server.ServerProfiles.Remove(userToRemove);

            await Context.Servers.UpdateAsync(server);
        }
    }
}
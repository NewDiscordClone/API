using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.BanUser
{
    public class BanUserCommandHandler : RequestHandlerBase, IRequestHandler<BanUserCommand>
    {
        public BanUserCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task Handle(BanUserCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(command.ServerId);

            if (server.Owner != UserId) //TODO: Замінити на логіку перевірки claim
                throw new NoPermissionsException("You don't have the appropriate rights");

            ServerProfile? userToRemove = server.ServerProfiles.Find(sp => sp.UserId == command.UserId);
            if (userToRemove != null)
                server.ServerProfiles.Remove(userToRemove);
            server.BannedUsers.Add(command.UserId);

            await Context.Servers.UpdateAsync(server);
        }
    }
}
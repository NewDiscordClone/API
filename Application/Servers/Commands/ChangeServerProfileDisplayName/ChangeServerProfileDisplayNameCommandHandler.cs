using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.ChangeServerProfileDisplayName
{
    public class ChangeServerProfileDisplayNameCommandHandler : RequestHandlerBase,
        IRequestHandler<ChangeServerProfileDisplayNameCommand>
    {
        public ChangeServerProfileDisplayNameCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider)
            : base(context, userProvider)
        {
        }

        public async Task Handle(ChangeServerProfileDisplayNameCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(command.ServerId);

            command.UserId ??= UserId;
            ServerProfile? serverProfile = server.ServerProfiles.Find(sp => sp.UserId == command.UserId) ??
                                           throw new Exception("The user are not a member of the server");
            if (serverProfile.UserId != UserId && server.Owner != UserId) //TODO: Замінити на логіку перевірки claim
                throw new NoPermissionsException("You are not allowed to change someone else's display name");
            serverProfile.DisplayName = command.NewDisplayName;
            await Context.Servers.UpdateAsync(server);
        }
    }
}
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Servers.Commands.ChangeServerProfileDisplayName
{
    public class ChangeServerProfileDisplayNameRequestHandler : RequestHandlerBase,
        IRequestHandler<ChangeServerProfileDisplayNameRequest>
    {
        public ChangeServerProfileDisplayNameRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider)
            : base(context, userProvider)
        {
        }

        public async Task Handle(ChangeServerProfileDisplayNameRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(request.ServerId);

            request.UserId ??= UserId;
            ServerProfile? serverProfile = server.ServerProfiles.Find(sp => sp.UserId == request.UserId) ??
                                           throw new Exception("The user are not a member of the server");
            if (serverProfile.UserId != UserId && server.Owner != UserId) //TODO: Замінити на логіку перевірки claim
                throw new NoPermissionsException("You are not allowed to change someone else's display name");
            serverProfile.DisplayName = request.NewDisplayName;
            await Context.Servers.UpdateAsync(server);
        }
    }
}
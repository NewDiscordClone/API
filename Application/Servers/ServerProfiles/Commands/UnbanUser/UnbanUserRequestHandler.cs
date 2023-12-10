using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.UnbanUser
{
    public class UnbanUserRequestHandler : RequestHandlerBase, IRequestHandler<UnbanUserCommand>
    {
        public UnbanUserRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task Handle(UnbanUserCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(request.ServerId);

            server.BannedUsers.Remove(request.UserId);

            await Context.Servers.UpdateAsync(server);
        }
    }
}
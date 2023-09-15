using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.LeaveServer
{
    public class LeaveServerCommandHandler : RequestHandlerBase, IRequestHandler<LeaveServerCommand>
    {
        public LeaveServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task Handle(LeaveServerCommand command, CancellationToken cancellationToken)
        {
            Server server = await Context.Servers.FindAsync(command.ServerId);
            ServerProfile serverProfile = server.ServerProfiles.Find(sp => sp.UserId == UserId) ??
                                          throw new NoPermissionsException("You are not a member of the server");
            server.ServerProfiles.Remove(serverProfile);
            await Context.Servers.UpdateAsync(server);
        }
    }
}
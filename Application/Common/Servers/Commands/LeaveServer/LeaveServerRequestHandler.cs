using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Common.Servers.Commands.LeaveServer
{
    public class LeaveServerRequestHandler : RequestHandlerBase, IRequestHandler<LeaveServerRequest>
    {
        public LeaveServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task Handle(LeaveServerRequest request, CancellationToken cancellationToken)
        {
            Server server = await Context.Servers.FindAsync(request.ServerId);
            ServerProfile serverProfile = server.ServerProfiles.Find(sp => sp.UserId == UserId) ??
                                          throw new NoPermissionsException("You are not a member of the server");
            server.ServerProfiles.Remove(serverProfile);
            await Context.Servers.UpdateAsync(server);
        }
    }
}
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Servers.LeaveServer
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
                                          throw new NoPermissionsException("You are not the member of the server");
            server.ServerProfiles.Remove(serverProfile);
            await Context.Servers.UpdateAsync(server);
        }
    }
}
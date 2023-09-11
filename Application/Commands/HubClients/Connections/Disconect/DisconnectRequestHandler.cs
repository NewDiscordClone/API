using Application.Application;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.HubClients.Connection.Disconnect
{
    public class DisconnectRequestHandler : HubRequestHandlerBase, IRequestHandler<DisconnectRequest>
    {
        public DisconnectRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context,
            IAuthorizedUserProvider userProvider) : base(hubContextProvider, context, userProvider)
        {
        }

        public async Task Handle(DisconnectRequest request, CancellationToken cancellationToken)
        {
            UserConnections? userConnections = await Context.UserConnections.FindOrDefaultAsync(UserId)!;

            if (userConnections == null) return;

            userConnections.Connections.Remove(request.ConnectionId);
            if (userConnections.Connections.Count == 0)
                await Context.UserConnections.DeleteAsync(userConnections);
            else
                await Context.UserConnections.UpdateAsync(userConnections);
        }
    }
}
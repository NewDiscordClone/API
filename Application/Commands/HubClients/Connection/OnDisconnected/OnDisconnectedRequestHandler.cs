using Application.Application;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.HubClients.Connection.OnDisconnected
{
    public class OnDisconnectedRequestHandler : HubRequestHandlerBase, IRequestHandler<OnDisconnectedRequest>
    {
        public OnDisconnectedRequestHandler(IHubContextProvider hubContextProvider,
            IAuthorizedUserProvider userProvider) : base(hubContextProvider, userProvider)
        {
        }

        public async Task Handle(OnDisconnectedRequest request, CancellationToken cancellationToken)
        {
            UserConnections? userConnections = await Context.UserConnections.FindOrDefaultAsync(UserId)!;

            if (userConnections == null) return;

            userConnections.Connections.Remove(request.ConnectionId);
            if (userConnections.Connections.Count == 0)
            {
                await Context.UserConnections.DeleteAsync(userConnections);
            }
        }
    }
}
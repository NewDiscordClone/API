using Application.Interfaces;
using Application.Providers;
using MediatR;

namespace Application.Commands.NotifyClients.OnConnected
{
    public class OnConnectedRequestHandler : HubRequestHandlerBase, IRequestHandler<OnConnectedRequest>
    {
        public OnConnectedRequestHandler(IHubContextProvider hubContextProvider, IAuthorizedUserProvider userProvider) : base(hubContextProvider, userProvider)
        {
        }

        public Task Handle(OnConnectedRequest request, CancellationToken cancellationToken)
        {
            if (!UserConnections.ContainsKey(UserId))
            {
                UserConnections[UserId] = new HashSet<string>();
            }
            UserConnections[UserId].Add(request.ConnectionId);
            return Task.CompletedTask;
        }
    }
}
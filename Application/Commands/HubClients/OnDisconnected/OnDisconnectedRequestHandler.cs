using Application.Interfaces;
using Application.Providers;
using MediatR;

namespace Application.Commands.NotifyClients.OnDisconnected
{
    public class OnDisconnectedRequestHandler: HubRequestHandlerBase, IRequestHandler<OnDisconnectedRequest>
    {
        public OnDisconnectedRequestHandler(IHubContextProvider hubContextProvider, IAuthorizedUserProvider userProvider) : base(hubContextProvider, userProvider)
        {
        }

        public Task Handle(OnDisconnectedRequest request, CancellationToken cancellationToken)
        {
            if (UserConnections.ContainsKey(UserId))
            {
                UserConnections[UserId].Remove(request.ConnectionId);
                if (UserConnections[UserId].Count == 0)
                {
                    UserConnections.Remove(UserId);
                }
            }
            return Task.CompletedTask;
        }
    }
}
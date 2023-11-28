using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Servers
{
    public class NotifyServerDeleted : HubHandler, INotificationHandler<ServerDeletedEvent>
    {
        public NotifyServerDeleted(IHubContextProvider hubContextProvider,
            IConnectionsRepository connectionsRepository)
            : base(hubContextProvider, connectionsRepository)
        {
        }

        public async Task Handle(ServerDeletedEvent notification, CancellationToken cancellationToken)
        {
            IEnumerable<string> connections = await ConnectionsRepository
                .FindConnectionsAsync(notification.Server, cancellationToken);

            await SendAsync(ClientMethods.ServerDeleted, notification.Server.Id, connections, cancellationToken);
        }
    }
}
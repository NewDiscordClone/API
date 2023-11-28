using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Servers
{
    public class NotifyServerUpdated : HubHandler,
        INotificationHandler<ServerUpdatedEvent>, INotificationHandler<ServerCreatedEvent>
    {
        public NotifyServerUpdated(IHubContextProvider hubContextProvider,
            IConnectionsRepository connectionsRepository)
            : base(hubContextProvider, connectionsRepository)
        {
        }

        private async Task Handle(Server server, CancellationToken cancellationToken)
        {
            IEnumerable<string> connections = await ConnectionsRepository
                .FindConnectionsAsync(server, cancellationToken);

            await SendAsync(ClientMethods.ServerUpdated, server, connections, cancellationToken);
        }

        public async Task Handle(ServerUpdatedEvent notification, CancellationToken cancellationToken)
           => await Handle(notification.Server, cancellationToken);

        public async Task Handle(ServerCreatedEvent notification, CancellationToken cancellationToken)
            => await Handle(notification.Server, cancellationToken);
    }
}
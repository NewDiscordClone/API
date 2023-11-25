using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Channels
{
    public class NotifyChannelRemoved : HubHandler, INotificationHandler<ChannelRemovedEvent>
    {
        public NotifyChannelRemoved(IHubContextProvider hubContextProvider,
            IConnectionsRepository connectionsRepository)
            : base(hubContextProvider, connectionsRepository)
        {
        }

        public async Task Handle(ChannelRemovedEvent notification, CancellationToken cancellationToken)
        {
            IEnumerable<string> connections = await ConnectionsRepository
                .FindConnectionsAsync(notification.Channel, cancellationToken);

            await SendAsync(ClientMethods.ChannelDeleted, notification.Channel, connections, cancellationToken);
        }
    }
}
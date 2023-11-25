using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Channels
{
    public class NotifyChannelUpdated : HubHandler, INotificationHandler<ChannelUpdatedEvent>
    {
        public NotifyChannelUpdated(IHubContextProvider hubContextProvider,
            IConnectionsRepository connectionsRepository)
            : base(hubContextProvider, connectionsRepository)
        {
        }

        public async Task Handle(ChannelUpdatedEvent notification, CancellationToken cancellationToken)
        {
            Channel channel = notification.Channel;
            IEnumerable<string> connections = await ConnectionsRepository
                .FindAsync(channel, cancellationToken);

            await SendAsync(ClientMethods.ChannelUpdated, channel, connections, cancellationToken);
        }
    }
}
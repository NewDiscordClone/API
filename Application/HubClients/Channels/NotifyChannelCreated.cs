using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Channels
{
    public class NotifyChannelCreated : HubHandler, INotificationHandler<ChannelCreatedEvent>
    {
        public NotifyChannelCreated(IHubContextProvider hubContextProvider, IConnectionsRepository connectionsRepository)
            : base(hubContextProvider, connectionsRepository)
        {
        }

        public async Task Handle(ChannelCreatedEvent createdEvent, CancellationToken cancellationToken)
        {
            Models.Channel channel = createdEvent.Channel;
            IEnumerable<string> connections = await ConnectionsRepository.FindAsync(channel, cancellationToken);

            await SendAsync(ClientMethods.ChannelCreated, channel, connections, cancellationToken);
        }
    }
}
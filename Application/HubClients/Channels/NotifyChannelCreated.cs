using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Channels
{
    public class NotifyChannelCreated : HubHandler, IRequestHandler<ChannelCreatedEvent>
    {
        public NotifyChannelCreated(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(ChannelCreatedEvent request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Channel channel = await Context.Channels.FindAsync(request.ChannelId);

            await SendAsync(ClientMethods.ChannelCreated, channel, GetConnections(channel));
        }
    }
}
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Channels.ChannelRemoved
{
    public class NotifyChannelRemovedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyChannelRemovedQuery>
    {
        public NotifyChannelRemovedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyChannelRemovedQuery query, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Channel channel = await Context.Channels.FindAsync(query.ChannelId);

            await SendAsync(ClientMethods.ChannelDeleted, channel, GetConnections(channel));
        }
    }
}
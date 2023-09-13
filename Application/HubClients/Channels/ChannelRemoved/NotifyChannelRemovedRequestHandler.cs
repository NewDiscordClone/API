using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Channels.ChannelRemoved
{
    public class NotifyChannelRemovedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyChannelRemovedRequest>
    {
        public NotifyChannelRemovedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyChannelRemovedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Channel channel = await Context.Channels.FindAsync(request.ChannelId);

            await SendAsync(ClientMethods.ChannelDeleted, channel, GetConnections(channel));
        }
    }
}
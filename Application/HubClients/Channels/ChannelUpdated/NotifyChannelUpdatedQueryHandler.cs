using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Channels.ChannelUpdated
{
    public class NotifyChannelUpdatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyChannelUpdatedQuery>
    {
        public NotifyChannelUpdatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyChannelUpdatedQuery request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Channel channel = await Context.Channels.FindAsync(request.ChannelId);

            await SendAsync(ClientMethods.ChannelUpdated, channel, GetConnections(channel));
        }
    }
}
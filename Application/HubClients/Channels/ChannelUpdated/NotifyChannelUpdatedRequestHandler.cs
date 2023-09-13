using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Channels.ChannelUpdated
{
    public class NotifyChannelUpdatedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyChannelUpdatedRequest>
    {
        public NotifyChannelUpdatedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyChannelUpdatedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Channel channel = await Context.Channels.FindAsync(request.ChannelId);

            await SendAsync(ClientMethods.ChannelUpdated, channel, GetConnections(channel));
        }
    }
}
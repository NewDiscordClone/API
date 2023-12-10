using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.Channels.ChannelCreated
{
    public class NotifyChannelCreatedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyChannelCreatedRequest>
    {
        public NotifyChannelCreatedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyChannelCreatedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Channel channel = await Context.Channels.FindAsync(request.ChannelId);

            await SendAsync(ClientMethods.ChannelCreated, channel, GetConnections(channel));
        }
    }
}
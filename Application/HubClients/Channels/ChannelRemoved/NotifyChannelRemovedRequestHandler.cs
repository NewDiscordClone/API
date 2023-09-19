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

            await SendAsync(ClientMethods.ChannelDeleted, new {serverId = request.Channel.ServerId, channelId = request.Channel.Id}, GetConnections(request.Channel));
        }
    }
}
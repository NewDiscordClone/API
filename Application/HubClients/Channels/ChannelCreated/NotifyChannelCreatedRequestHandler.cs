using Application.Application;
using Application.Common.Interfaces;
using Application.HubClients;
using Application.Models;
using MediatR;

namespace Application.HubClients.Channels.ChannelCreated
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
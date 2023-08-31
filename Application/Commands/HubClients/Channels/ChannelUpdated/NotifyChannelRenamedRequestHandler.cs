using Application.Application;
using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Commands.HubClients.Channels.UpdateChannel
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
            
            await SendAsync("ChannelRemoved", channel, GetConnections(channel));
        }
    }
}
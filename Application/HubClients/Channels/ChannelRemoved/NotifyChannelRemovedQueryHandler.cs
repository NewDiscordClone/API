using MediatR;
using Sparkle.Application.Common.Interfaces;

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

            await SendAsync(ClientMethods.ChannelDeleted, query.Channel, GetConnections(query.Channel));
        }
    }
}
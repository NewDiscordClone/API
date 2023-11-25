using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Common;

namespace Sparkle.Application.HubClients.Channels
{
    public class NotifyChannelRemovedQueryHandler : HubHandler, IRequestHandler<NotifyChannelRemovedQuery>
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
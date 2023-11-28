using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Common;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatRemoved
{
    public class NotifyPrivateChatRemovedQueryHandler : HubHandler, IRequestHandler<NotifyPrivateChatRemovedQuery>
    {
        public NotifyPrivateChatRemovedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyPrivateChatRemovedQuery query, CancellationToken cancellationToken)
        {
            await SendAsync(ClientMethods.PrivateChatRemoved, query.ChatId, GetConnectionsAsync(query.UserId));
        }
    }
}
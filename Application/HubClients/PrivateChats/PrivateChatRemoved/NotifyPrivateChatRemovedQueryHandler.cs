using MediatR;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatRemoved
{
    public class NotifyPrivateChatRemovedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyPrivateChatRemovedQuery>
    {
        public NotifyPrivateChatRemovedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyPrivateChatRemovedQuery query, CancellationToken cancellationToken)
        {
            await SendAsync(ClientMethods.PrivateChatRemoved, query.ChatId, GetConnections(query.UserId));
        }
    }
}
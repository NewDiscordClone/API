using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatUpdated
{
    public class NotifyPrivateChatUpdatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyPrivateChatUpdatedQuery>
    {
        public NotifyPrivateChatUpdatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyPrivateChatUpdatedQuery query, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Chat chat = await Context.Chats.FindAsync(query.ChatId);

            await SendAsync(ClientMethods.PrivateChatUpdated, chat, GetConnections(chat));
        }
    }
}
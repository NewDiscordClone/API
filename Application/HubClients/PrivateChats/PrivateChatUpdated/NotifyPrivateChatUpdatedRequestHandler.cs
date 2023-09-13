using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatUpdated
{
    public class NotifyPrivateChatUpdatedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyPrivateChatUpdatedRequest>
    {
        public NotifyPrivateChatUpdatedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyPrivateChatUpdatedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Chat chat = await Context.Chats.FindAsync(request.ChatId);

            await SendAsync(ClientMethods.PrivateChatUpdated, chat, GetConnections(chat));
        }
    }
}
using Application.Application;
using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Commands.HubClients.PrivateChats.PrivateChatUpdated
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
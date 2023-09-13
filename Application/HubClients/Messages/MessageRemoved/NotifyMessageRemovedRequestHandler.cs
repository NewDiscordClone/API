using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Messages.MessageRemoved
{
    public class NotifyMessageRemovedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyMessageRemovedRequest>
    {
        public NotifyMessageRemovedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyMessageRemovedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Chat chat = await Context.Chats.FindAsync(request.ChatId);
            await SendAsync(ClientMethods.MessageDeleted, request, GetConnections(chat));
        }
    }
}
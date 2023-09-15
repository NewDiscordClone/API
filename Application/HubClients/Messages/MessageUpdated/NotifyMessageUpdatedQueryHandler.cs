using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Messages.MessageUpdated
{
    public class NotifyMessageUpdatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyMessageUpdatedQuery>
    {
        public async Task Handle(NotifyMessageUpdatedQuery query, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(query.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);

            await SendAsync(ClientMethods.MessageUpdated, message, GetConnections(chat));
        }

        public NotifyMessageUpdatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) :
            base(hubContextProvider, context, mapper)
        { }
    }
}
using Application.Application;
using Application.Common.Interfaces;
using Application.HubClients;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.HubClients.Messages.MessageUpdated
{
    public class NotifyMessageUpdatedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyMessageUpdatedRequest>
    {
        public async Task Handle(NotifyMessageUpdatedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(request.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);

            await SendAsync(ClientMethods.MessageUpdated, message, GetConnections(chat));
        }

        public NotifyMessageUpdatedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) :
            base(hubContextProvider, context, mapper)
        { }
    }
}
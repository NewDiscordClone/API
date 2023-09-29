using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Messages.MessageAdded
{
    public class NotifyMessageAddedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyMessageAddedQuery>
    {
        public async Task Handle(NotifyMessageAddedQuery query, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);

            MessageDto messageDto = query.MessageDto;

            Chat chat = await Context.Chats.FindAsync(messageDto.ChatId, cancellationToken);
            if (chat is Channel channel)
                messageDto.ServerId = channel.ServerId;

            await SendAsync(ClientMethods.MessageAdded, messageDto, GetConnections(chat));
        }

        public NotifyMessageAddedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) :
            base(hubContextProvider, context, mapper)
        { }
    }
}
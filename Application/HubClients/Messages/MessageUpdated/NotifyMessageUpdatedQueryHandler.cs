using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Messages.MessageUpdated
{
    public class NotifyMessageUpdatedQueryHandler : HubHandler, IRequestHandler<NotifyMessageUpdatedQuery>
    {
        public async Task Handle(NotifyMessageUpdatedQuery query, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Message message = await Context.Messages.FindAsync(query.MessageId, cancellationToken);
            MessageDto messageDto = Mapper.Map<MessageDto>(message);


            Chat chat = await Context.Chats.FindAsync(message.ChatId, cancellationToken);
            if (chat is Channel channel)
                messageDto.ServerId = channel.ServerId;

            await SendAsync(ClientMethods.MessageUpdated, messageDto, GetConnections(chat));
        }

        public NotifyMessageUpdatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) :
            base(hubContextProvider, context, mapper)
        {
        }
    }
}
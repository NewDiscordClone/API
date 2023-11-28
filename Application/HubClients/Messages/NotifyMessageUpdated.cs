using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Messages
{
    public class NotifyMessageUpdated : HubHandler, INotificationHandler<MessageUpdatedEvent>
    {
        private readonly IChatRepository _chatRepository;
        public async Task Handle(MessageUpdatedEvent notification, CancellationToken cancellationToken)
        {
            Message message = notification.Message;
            MessageDto messageDto = Mapper.Map<MessageDto>(message);


            Chat chat = await _chatRepository.FindAsync(message.ChatId, cancellationToken);
            if (chat is Channel channel)
                messageDto.ServerId = channel.ServerId;

            IEnumerable<string> connections = await ConnectionsRepository
                .FindConnectionsAsync(chat, cancellationToken);

            await SendAsync(ClientMethods.MessageUpdated, messageDto, connections, cancellationToken);
        }

        public NotifyMessageUpdated(IHubContextProvider hubContextProvider,
            IConnectionsRepository connectionsRepository,
            IMapper mapper,
            IChatRepository chatRepository) :
            base(hubContextProvider, mapper, connectionsRepository)
        {
            _chatRepository = chatRepository;
        }
    }
}
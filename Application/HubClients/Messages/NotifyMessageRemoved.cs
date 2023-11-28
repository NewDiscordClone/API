using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Messages
{
    public class NotifyMessageRemoved : HubHandler, INotificationHandler<MessageRemovedEvent>
    {
        private readonly IChatRepository _chatRepository;
        public NotifyMessageRemoved(IHubContextProvider hubContextProvider,
            IConnectionsRepository connectionsRepository,
            IChatRepository chatRepository)
            : base(hubContextProvider, connectionsRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task Handle(MessageRemovedEvent notification, CancellationToken cancellationToken)
        {
            Message message = notification.Message;

            Chat chat = await _chatRepository.FindAsync(message.ChatId, cancellationToken);
            IEnumerable<string> connections = await ConnectionsRepository
                .FindConnectionsAsync(chat, cancellationToken);

            (string chatId, string messageId) messageData = (message.ChatId, message.Id);

            await SendAsync(ClientMethods.MessageDeleted, messageData, connections, cancellationToken);
        }
    }
}
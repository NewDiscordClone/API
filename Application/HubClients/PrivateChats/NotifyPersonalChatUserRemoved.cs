using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.PrivateChats
{
    public class NotifyPersonalChatUserRemoved : HubHandler, INotificationHandler<PersonalChatUserRemoved>
    {
        public NotifyPersonalChatUserRemoved(IHubContextProvider hubContextProvider,
            IConnectionsRepository connectionsRepository) : base(hubContextProvider, connectionsRepository)
        {
        }

        public async Task Handle(PersonalChatUserRemoved notification, CancellationToken cancellationToken)
        {
            IEnumerable<string> connections = await ConnectionsRepository
                .FindConnectionsAsync(notification.RemovedUserId, cancellationToken);

            await SendAsync(ClientMethods.PrivateChatRemoved, notification.Chat.Id, connections, cancellationToken);
        }
    }
}
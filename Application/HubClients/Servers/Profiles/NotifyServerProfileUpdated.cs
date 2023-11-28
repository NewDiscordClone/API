using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Servers.Profiles
{
    public class NotifyServerProfileUpdated : HubHandler, INotificationHandler<ProfileUpdatedEvent>
    {
        private readonly IServerProfileRepository _repository;
        public NotifyServerProfileUpdated(IHubContextProvider hubContextProvider,
            IServerProfileRepository repository,
            IConnectionsRepository connectionsRepository)
            : base(hubContextProvider, connectionsRepository)
        {
            _repository = repository;
        }

        public async Task Handle(ProfileUpdatedEvent notification, CancellationToken cancellationToken)
        {
            List<Guid> userIds = await _repository
                .GetUserIdsFromServer(notification.Profile.ServerId, cancellationToken);

            await SendAsync(ClientMethods.ProfileSaved, notification.Profile, userIds, cancellationToken);
        }
    }
}

using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Servers.Profiles
{
    public class NotifyServerProfileDeleted : HubHandler, INotificationHandler<ProfileDeletedEvent>
    {
        private readonly IServerProfileRepository _repository;
        public NotifyServerProfileDeleted(IHubContextProvider hubContextProvider,
            IConnectionsRepository connectionRepository,
            IServerProfileRepository repository)
            : base(hubContextProvider, connectionRepository)
        {
            _repository = repository;
        }

        public async Task Handle(ProfileDeletedEvent notification, CancellationToken cancellationToken)
        {
            List<Guid> userIds = await _repository
                .GetUserIdsFromServer(notification.Profile.ServerId, cancellationToken);

            await SendAsync(ClientMethods.ProfileDeleted, notification.Profile, userIds, cancellationToken);
        }
    }
}

using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Roles
{
    public class NotifyRoleSaved : HubHandler,
        INotificationHandler<RoleUpdatedEvent>, INotificationHandler<RoleCreatedEvent>
    {
        private readonly IServerProfileRepository _profileRepository;

        private async Task Handle(Role role, CancellationToken cancellationToken)
        {
            List<Guid> userIds = await _profileRepository
                            .GetUserIdsFromServer(role.ServerId!, cancellationToken);

            await SendAsync(ClientMethods.RoleSaved, role, userIds, cancellationToken);
        }

        public async Task Handle(RoleUpdatedEvent notification, CancellationToken cancellationToken)
            => await Handle(notification.Role, cancellationToken);

        public async Task Handle(RoleCreatedEvent notification, CancellationToken cancellationToken)
            => await Handle(notification.Role, cancellationToken);

        public NotifyRoleSaved(IHubContextProvider hubContextProvider,
          IServerProfileRepository profileRepository,
          IConnectionsRepository connectionsRepository)
          : base(hubContextProvider, connectionsRepository)
        {
            _profileRepository = profileRepository;
        }
    }
}

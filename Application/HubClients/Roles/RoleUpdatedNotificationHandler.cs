using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Roles
{
    public class RoleUpdatedNotificationHandler : HubHandler, INotificationHandler<RoleUpdatedEvent>
    {
        private readonly IServerProfileRepository _profileRepository;
        public RoleUpdatedNotificationHandler(IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IServerProfileRepository profileRepository)
            : base(hubContextProvider, context)
        {
            _profileRepository = profileRepository;
        }

        public async Task Handle(RoleUpdatedEvent notification, CancellationToken cancellationToken)
        {
            List<Guid> userIds = await _profileRepository
                .GetUserIdsFromServer(notification.Role.ServerId!, cancellationToken);

            await SendAsync(ClientMethods.RoleSaved, notification.Role, userIds);
        }
    }
}

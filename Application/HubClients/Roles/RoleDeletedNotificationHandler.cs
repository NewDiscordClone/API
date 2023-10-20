using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Roles
{
    public class RoleDeletedNotificationHandler : HubRequestHandlerBase, INotificationHandler<RoleDeletedEvent>
    {
        private readonly IUserProfileRepository _profileRepository;

        public RoleDeletedNotificationHandler(IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IUserProfileRepository profileRepository)
            : base(hubContextProvider, context)
        {
            _profileRepository = profileRepository;
        }

        public async Task Handle(RoleDeletedEvent notification, CancellationToken cancellationToken)
        {
            List<Guid> userIds = await _profileRepository
              .GetUserIdsFromServer(notification.Role.ServerId!, cancellationToken);

            await SendAsync(ClientMethods.RoleDeleted, notification.Role, userIds);
        }
    }
}

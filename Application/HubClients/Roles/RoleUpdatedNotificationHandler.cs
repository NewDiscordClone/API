using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Roles
{
    public class RoleUpdatedNotificationHandler : HubRequestHandlerBase, INotificationHandler<RoleUpdatedEvent>
    {
        private readonly IUserProfileRepository _profileRepository;
        public RoleUpdatedNotificationHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(RoleUpdatedEvent notification, CancellationToken cancellationToken)
        {
            List<Guid> userIds = await _profileRepository
                .GetUserIdsFromServer(notification.Role.ServerId!, cancellationToken);

            await SendAsync(ClientMethods.RoleUpdated, notification.Role, userIds);
        }
    }
}

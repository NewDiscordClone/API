using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Domain.Events;

namespace Sparkle.Application.HubClients.Servers.Profiles
{
    public class ServerProfileUpdatedNotificationHandler : HubRequestHandlerBase, INotificationHandler<ProfileUpdatedEvent>
    {
        private readonly IServerProfileRepository _repository;
        public ServerProfileUpdatedNotificationHandler(IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IServerProfileRepository repository)
            : base(hubContextProvider, context)
        {
            _repository = repository;
        }

        public async Task Handle(ProfileUpdatedEvent notification, CancellationToken cancellationToken)
        {
            List<Guid> userIds = await _repository
                .GetUserIdsFromServer(notification.Profile.ServerId, cancellationToken);

            await SendAsync(ClientMethods.ProfileSaved, notification.Profile, userIds);
        }
    }
}

using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models.Events;

namespace Sparkle.Application.HubClients.Servers.Profiles
{
    public class ServerProfileDeletedNotificationHandler : HubRequestHandlerBase, INotificationHandler<ProfileDeletedEvent>
    {
        private readonly IServerProfileRepository _repository;
        public ServerProfileDeletedNotificationHandler(IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IServerProfileRepository repository)
            : base(hubContextProvider, context)
        {
            _repository = repository;
        }

        public async Task Handle(ProfileDeletedEvent notification, CancellationToken cancellationToken)
        {
            List<Guid> userIds = await _repository
                .GetUserIdsFromServer(notification.Profile.ServerId, cancellationToken);

            await SendAsync(ClientMethods.ProfileUpdated, notification.Profile, userIds);
        }
    }
}

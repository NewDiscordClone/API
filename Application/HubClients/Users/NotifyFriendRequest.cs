using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Users
{
    public class NotifyFriendRequest : HubHandler, INotificationHandler<FriendRequestEvent>
    {
        private readonly IUserRepository _userRepository;
        public NotifyFriendRequest(IHubContextProvider hubContextProvider,
            IConnectionsRepository connectionsRepository,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IUserRepository userRepository) : base(hubContextProvider, userProvider, mapper, connectionsRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(FriendRequestEvent notifications, CancellationToken cancellationToken)
        {
            User user = await _userRepository.FindAsync(UserId, cancellationToken);

            IEnumerable<string> connections = await ConnectionsRepository
                .FindConnectionsAsync(notifications.Relationship.Passive, cancellationToken);

            await SendAsync(ClientMethods.FriendRequest, Mapper.Map<UserViewModel>(user), connections, cancellationToken);
        }
    }
}
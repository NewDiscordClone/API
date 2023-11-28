using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Users
{
    public class NotifyUserUpdated : HubHandler, INotificationHandler<UserUpdatedEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IRelationshipRepository _relationshipRepository;

        public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            List<UserProfile> profiles = await _userRepository.ExecuteCustomQuery(users => users
                .Where(user => user.Id == notification.User.Id)
                .SelectMany(user => user.UserProfiles))
                .ToListAsync(cancellationToken);

            IEnumerable<string?> chatIds = profiles.Select(profile => profile.ChatId);
            List<Chat> chats = await _chatRepository.ExecuteCustomQuery(chats => chats
                .Where(chat => chatIds.Contains(chat.Id)))
                .ToListAsync(cancellationToken);

            List<string> connections = [];

            foreach (Chat chat in chats)
            {
                IEnumerable<string> chatConnections = await ConnectionsRepository
                    .FindConnectionsAsync(chat, cancellationToken);

                connections.AddRange(chatConnections);
            }

            List<Relationship> relationships = await _relationshipRepository.ExecuteCustomQuery(relationship => relationship
                .Where(r => r.Active == UserId || r.Passive == UserId))
                .ToListAsync(cancellationToken);

            foreach (Relationship relationship in relationships)
            {
                Guid userId = relationship.Active != UserId ? relationship.Active : relationship.Passive;
                IEnumerable<string> relationshipConnections = await ConnectionsRepository
                    .FindConnectionsAsync(userId, cancellationToken);

                connections.AddRange(relationshipConnections);
            }

            IEnumerable<string> serverIds = profiles.OfType<ServerProfile>().Select(profile => profile.ServerId);
            List<Server> servers = await _serverRepository.ExecuteCustomQuery(servers => servers
                .Where(server => serverIds.Contains(server.Id)))
                .ToListAsync(cancellationToken);

            foreach (Server server in servers)
            {
                IEnumerable<string> serverConnections = await ConnectionsRepository
                    .FindConnectionsAsync(server, cancellationToken);

                connections.AddRange(serverConnections);
            }

            UserViewModel notifyArg = Mapper.Map<UserViewModel>(notification.User);

            IEnumerable<string> userConnections = await ConnectionsRepository
                .FindConnectionsAsync(UserId, cancellationToken);

            await SendAsync(ClientMethods.UserUpdated, notifyArg, userConnections, cancellationToken);
            await SendAsync(ClientMethods.UserUpdated, notifyArg, connections, cancellationToken);
        }

        public NotifyUserUpdated(IHubContextProvider hubContextProvider,
          IAuthorizedUserProvider userProvider,
          IConnectionsRepository connectionsRepository,
          IMapper mapper,
          IUserRepository userRepository,
          IChatRepository chatRepository,
          IRelationshipRepository relationshipRepository,
          IServerRepository serverRepository)
          : base(hubContextProvider, userProvider, mapper, connectionsRepository)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _relationshipRepository = relationshipRepository;
            _serverRepository = serverRepository;
        }
    }
}
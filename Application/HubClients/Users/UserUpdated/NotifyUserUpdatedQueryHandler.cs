using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;
using Sparkle.Domain.LookUps;

namespace Sparkle.Application.HubClients.Users.UserUpdated
{
    public class NotifyUserUpdatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyUserUpdatedQuery>
    {
        public NotifyUserUpdatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context,
            IAuthorizedUserProvider userProvider, IMapper mapper) : base(hubContextProvider, context, userProvider,
            mapper)
        {
        }

        public async Task Handle(NotifyUserUpdatedQuery query, CancellationToken cancellationToken)
        {
            List<UserProfile> profiles = await Context.Users
                .Where(user => user.Id == query.UpdatedUser.Id)
                .SelectMany(user => user.UserProfiles)
                .ToListAsync(cancellationToken);


            IEnumerable<string?> chatIds = profiles.Select(profile => profile.ChatId);
            List<PersonalChat> chats = await Context.PersonalChats
                .FilterAsync(chat => chatIds.Contains(chat.Id), cancellationToken);

            List<string> connections = new();

            foreach (Chat chat in chats)
            {
                connections.AddRange(GetConnections(chat));
            }

            List<Relationship> relationships = await Context.Relationships
                .Where(r => r.Active == UserId || r.Passive == UserId)
                .ToListAsync(cancellationToken);

            foreach (Relationship relationship in relationships)
            {
                connections.AddRange(GetConnections(
                    relationship.Active != UserId ? relationship.Active : relationship.Passive
                ));
            }

            IEnumerable<string> serverIds = profiles.OfType<ServerProfile>().Select(profile => profile.ServerId);
            List<Server> servers = await Context.Servers
                .FilterAsync(server => serverIds.Contains(server.Id), cancellationToken);

            foreach (Server server in servers)
            {
                connections.AddRange(GetConnections(server));
            }

            UserViewModel notifyArg = Mapper.Map<UserViewModel>(query.UpdatedUser);

            await SendAsync(ClientMethods.UserUpdated, notifyArg, GetConnections(UserId));
            await SendAsync(ClientMethods.UserUpdated, notifyArg, connections);
        }
    }
}
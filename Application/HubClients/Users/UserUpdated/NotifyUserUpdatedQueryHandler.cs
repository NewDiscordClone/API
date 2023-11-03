using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

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

            List<Chat> chats = await Context.Chats
                .FilterAsync(chat => profiles.Any(profile =>
                profile.ChatId == chat.Id), cancellationToken);

            List<string> connections = new();

            foreach (Chat chat in chats)
            {
                connections.AddRange(GetConnections(chat));
            }

            List<Server> servers = await Context.Servers
                .FilterAsync(server => profiles.OfType<ServerProfile>()
                .Any(profile => profile.ServerId == server.Id), cancellationToken);

            foreach (Server server in servers)
            {
                connections.AddRange(GetConnections(server));
            }

            UserViewModel notifyArg = Mapper.Map<UserViewModel>(profiles);

            await SendAsync(ClientMethods.UserUpdated, notifyArg, GetConnections(UserId));
            await SendAsync(ClientMethods.UserUpdated, notifyArg, connections);

        }
    }
}
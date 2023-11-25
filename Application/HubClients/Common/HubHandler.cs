using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Common
{
    public abstract class HubHandler : RequestHandler
    {
        private readonly IHubContextProvider _hubContextProvider;
        protected IHubClients Clients => _hubContextProvider.Clients;

        protected IEnumerable<string> GetConnections(Guid userId)
        {
            return Context.UserConnections.FindOrDefaultAsync(userId).Result?.Connections ?? new HashSet<string>();
        }
        protected IEnumerable<string> GetConnections(IEnumerable<Guid> userIds)
        {
            return userIds
               .Where(userId => Context.UserConnections.FindOrDefaultAsync(userId)?.Result != null)
               .SelectMany(userId => Context.UserConnections.FindAsync(userId).Result.Connections);
        }
        protected IEnumerable<string> GetConnections(params Guid[] userIds)
        {
            return GetConnections(userIds.AsEnumerable());
        }
        protected IEnumerable<string> GetConnections(Chat chat)
        {
            Guid[] userIds = chat switch
            {
                PersonalChat p => GetPersonalChatUserIds(p),
                Channel c => GetChannelUserIds(c),
                _ => throw new Exception("Unknown chat type")
            };

            return GetConnections(userIds);
        }

        private Guid[] GetChannelUserIds(Channel cannel)
        {
            return Context.UserProfiles
                  .OfType<ServerProfile>()
                  .Where(profile => profile.ServerId == cannel.ServerId)
                  .Select(profile => profile.UserId)
                  .ToArray();
        }

        private Guid[] GetPersonalChatUserIds(PersonalChat chat)
        {
            return Context.UserProfiles
                  .Where(profile => profile.ChatId == chat.Id)
                  .Select(profile => profile.UserId)
                  .ToArray();
        }

        protected IEnumerable<string> GetConnections(Server server)
        {
            Guid[] userIds = Context.UserProfiles
                .OfType<ServerProfile>()
              .Where(profile => profile.ServerId == server.Id)
              .Select(profile => profile.UserId)
              .ToArray();

            return GetConnections(userIds);
        }

        protected async Task SendAsync<T>(string method, T arg, IEnumerable<string> connections, CancellationToken cancellationToken = default)
        {
            IReadOnlyList<string> readOnlyConnections = connections.ToList().AsReadOnly();
            await Clients.Clients(readOnlyConnections).SendAsync(method, arg, cancellationToken);
        }
        protected async Task SendAsync<T>(string method, T arg, IEnumerable<Guid> userIds)
        {
            IEnumerable<string> connections = GetConnections(userIds);
            await SendAsync(method, arg, connections);
        }

        protected HubHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(context)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) :
            base(context, mapper)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubHandler(IHubContextProvider hubContextProvider, IAppDbContext context,
            IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubHandler(IHubContextProvider hubContextProvider, IAppDbContext context,
            IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
            _hubContextProvider = hubContextProvider;
        }
    }
}
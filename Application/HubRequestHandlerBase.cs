using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace Application.Application
{
    public abstract class HubRequestHandlerBase : RequestHandlerBase
    {
        private readonly IHubContextProvider _hubContextProvider;
        protected IHubClients Clients => _hubContextProvider.Clients;

        private CancellationToken _token;

        protected IEnumerable<string> GetConnections(User user)
        {
            return Context.UserConnections.FindAsync(user.Id).Result.Connections;
        }
        protected IEnumerable<string> GetConnections(Chat chat)
        {
            return chat.Users
                .Where(user => Context.UserConnections.FindOrDefaultAsync(user.Id).Result != null)
                .SelectMany(user => Context.UserConnections.FindAsync(user.Id).Result.Connections);
        }

        protected IEnumerable<string> GetConnections(Server server)
        {
            return server.ServerProfiles
                .Where(sp => Context.UserConnections.FindOrDefaultAsync(sp.User.Id).Result != null)
                .SelectMany(sp => Context.UserConnections.FindAsync(sp.User.Id).Result.Connections);
        }

        protected void SetToken(CancellationToken cancellationToken)
        {
            _token = cancellationToken;
            Context?.SetToken(cancellationToken);
        }

        protected async Task SendAsync<T>(string method, T arg, IEnumerable<string> connections)
        {
            await Clients.Clients(connections).SendAsync(method, arg, _token);
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAppDbContext context) : base(context)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAuthorizedUserProvider userProvider) :
            base(userProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) :
            base(context, mapper)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAppDbContext context,
            IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAppDbContext context,
            IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
            _hubContextProvider = hubContextProvider;
        }
    }
}
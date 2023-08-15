using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace Application.Commands.NotifyClients
{
    public abstract class HubRequestHandlerBase : RequestHandlerBase
    {
        private readonly IHubContextProvider _hubContextProvider;
        protected IHubClients Clients => _hubContextProvider.Clients;
        protected static readonly Dictionary<int, HashSet<string>> UserConnections = new();

        protected IEnumerable<string> GetConnections(Chat chat)
        {
            return chat.Users
                .Where(user => UserConnections.ContainsKey(user.Id))
                .SelectMany(user => UserConnections[user.Id]);
        }
        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IMapper mapper) : base(mapper)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAppDbContext context) : base(context)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAuthorizedUserProvider userProvider) : base(userProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAuthorizedUserProvider userProvider, IMapper mapper) : base(userProvider, mapper)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
            _hubContextProvider = hubContextProvider;
        }

        protected HubRequestHandlerBase(IHubContextProvider hubContextProvider, IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
            _hubContextProvider = hubContextProvider;
        }
    }
}
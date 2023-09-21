﻿using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients
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
            return chat.Profiles
                .Where(profile => Context.UserConnections.FindOrDefaultAsync(profile.UserId)?.Result != null)
                .SelectMany(profile => Context.UserConnections.FindAsync(profile.UserId).Result.Connections);
        }

        protected IEnumerable<string> GetConnections(Server server)
        {
            return server.ServerProfiles
                .Where(sp => Context.UserConnections.FindOrDefaultAsync(sp.UserId)?.Result != null)
                .SelectMany(sp => Context.UserConnections.FindAsync(sp.UserId).Result.Connections);
        }

        protected void SetToken(CancellationToken cancellationToken)
        {
            _token = cancellationToken;
            Context?.SetToken(cancellationToken);
        }

        protected async Task SendAsync<T>(string method, T arg, IEnumerable<string> connections)
        {
            IReadOnlyList<string> readOnlyConnections = connections.ToList().AsReadOnly();
            await Clients.Clients(readOnlyConnections).SendAsync(method, arg, _token);
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
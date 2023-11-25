using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;

namespace Sparkle.Application.HubClients.Common
{
    public abstract class HubHandler : RequestHandler
    {
        private readonly IHubContextProvider _hubContextProvider;
        protected IHubClients Clients => _hubContextProvider.Clients;
        protected readonly IConnectionsRepository ConnectionsRepository;

        protected async Task SendAsync<T>(string method, T arg, IEnumerable<string> connections, CancellationToken cancellationToken = default)
        {
            IReadOnlyList<string> readOnlyConnections = connections.ToList().AsReadOnly();
            await Clients.Clients(readOnlyConnections).SendAsync(method, arg, cancellationToken);
        }
        protected async Task SendAsync<T>(string method, T arg, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
        {
            IEnumerable<string> connections = await ConnectionsRepository.FindConnectionsAsync(userIds, cancellationToken);
            await SendAsync(method, arg, connections, cancellationToken);
        }

        protected HubHandler(IHubContextProvider hubContextProvider, IConnectionsRepository connectionsRepository)
        {
            _hubContextProvider = hubContextProvider;
            ConnectionsRepository = connectionsRepository;
        }

        protected HubHandler(IHubContextProvider hubContextProvider, IMapper mapper, IConnectionsRepository connectionsRepository) :
            base(mapper)
        {
            _hubContextProvider = hubContextProvider;
            ConnectionsRepository = connectionsRepository;
        }

        protected HubHandler(IHubContextProvider hubContextProvider,
            IAuthorizedUserProvider userProvider,
            IConnectionsRepository connectionsRepository) : base(userProvider)
        {
            _hubContextProvider = hubContextProvider;
            ConnectionsRepository = connectionsRepository;
        }

        protected HubHandler(IHubContextProvider hubContextProvider,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IConnectionsRepository connectionsRepository) : base(userProvider, mapper)
        {
            _hubContextProvider = hubContextProvider;
            ConnectionsRepository = connectionsRepository;
        }
    }
}
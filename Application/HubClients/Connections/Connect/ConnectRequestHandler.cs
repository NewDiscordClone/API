using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Connections.Connect
{
    public class ConnectRequestHandler : HubRequestHandlerBase, IRequestHandler<ConnectRequest>
    {
        private readonly IHubContextProvider _hubContextProvider;

        public ConnectRequestHandler(
            IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(hubContextProvider, context, userProvider, mapper)
        {
            _hubContextProvider = hubContextProvider;
        }

        public async Task Handle(ConnectRequest request, CancellationToken cancellationToken)
        {
            UserConnections? userConnections = await Context.UserConnections.FindOrDefaultAsync(UserId);
            if (userConnections == null)
            {
                userConnections = new UserConnections
                { UserId = UserId, Connections = new HashSet<string>() { request.ConnectionId } };
                await Context.UserConnections.AddAsync(userConnections);
                User user = await Context.Users.FindAsync(UserId)!;
                user.Status = UserStatus.Online;
                await Context.SqlUsers.UpdateAsync(user);
            }
            else
            {
                userConnections.Connections.Add(request.ConnectionId);
                await Context.UserConnections.UpdateAsync(userConnections);
            }
        }
    }
}
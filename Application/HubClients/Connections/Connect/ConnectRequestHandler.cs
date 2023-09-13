using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Connections.Connect
{
    public class ConnectRequestHandler : HubRequestHandlerBase, IRequestHandler<ConnectRequest>
    {
        public ConnectRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IAuthorizedUserProvider userProvider) : base(hubContextProvider, context, userProvider)
        {
        }

        public async Task Handle(ConnectRequest request, CancellationToken cancellationToken)
        {
            UserConnections? userConnections = await Context.UserConnections.FindOrDefaultAsync(UserId)!;
            if (userConnections == null)
            {
                userConnections = new UserConnections
                { UserId = UserId, Connections = new HashSet<string>() { request.ConnectionId } };
                await Context.UserConnections.AddAsync(userConnections);
            }
            else
            {
                userConnections.Connections.Add(request.ConnectionId);
                await Context.UserConnections.UpdateAsync(userConnections);
            }
        }
    }
}
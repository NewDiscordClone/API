using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Users.UserUpdated;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Connections.Disconect
{
    public class DisconnectRequestHandler : HubRequestHandlerBase, IRequestHandler<DisconnectRequest>
    {
        private readonly IHubContextProvider _hubContextProvider;

        public DisconnectRequestHandler(
            IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper) : 
            base(hubContextProvider, context, userProvider,mapper)
        {
            _hubContextProvider = hubContextProvider;
        }

        public async Task Handle(DisconnectRequest request, CancellationToken cancellationToken)
        {
            UserConnections? userConnections = await Context.UserConnections.FindOrDefaultAsync(UserId)!;

            if (userConnections == null)
                return;

            userConnections.Connections.Remove(request.ConnectionId);
            if (userConnections.Connections.Count == 0)
            {
                await Context.UserConnections.DeleteAsync(userConnections);
                User user = await Context.SqlUsers.FindAsync(UserId);
                user.Status = UserStatus.Offline;
                await Context.SqlUsers.UpdateAsync(user);
                await new NotifyUserUpdatedRequestHandler(_hubContextProvider, Context, UserProvider, Mapper)
                    .Handle(new NotifyUserUpdatedRequest(), cancellationToken);
            }
            else
                await Context.UserConnections.UpdateAsync(userConnections);
        }
    }
}
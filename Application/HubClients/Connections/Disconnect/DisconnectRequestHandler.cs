using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Connections.Disconnect
{
    public class DisconnectRequestHandler : HubRequestHandlerBase, IRequestHandler<DeleteUserConnectionCommand, User?>
    {
        private readonly IRepository<User, Guid> _userRepository;
        public DisconnectRequestHandler(
            IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IRepository<User, Guid> userRepository) :
            base(hubContextProvider, context, userProvider, mapper)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> Handle(DeleteUserConnectionCommand request, CancellationToken cancellationToken)
        {
            UserConnections? userConnections = await Context.UserConnections
                .FindOrDefaultAsync(UserId, cancellationToken)!;

            User? user = null;

            if (userConnections == null)
                return null;

            userConnections.Connections.Remove(request.ConnectionId);
            if (userConnections.Connections.Count == 0)
            {
                await Context.UserConnections.DeleteAsync(userConnections, cancellationToken);

                user = await _userRepository.FindAsync(UserId, cancellationToken);
                user.Status = UserStatus.Offline;

                await Context.SqlUsers.UpdateAsync(user, cancellationToken);
            }
            else
                await Context.UserConnections.UpdateAsync(userConnections, cancellationToken);

            return user;
        }
    }
}
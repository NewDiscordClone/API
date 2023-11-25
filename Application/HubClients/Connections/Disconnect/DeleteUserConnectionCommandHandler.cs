using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Connections.Disconnect
{
    public class DeleteUserConnectionCommandHandler : HubHandler, IRequestHandler<DeleteUserConnectionCommand, User?>
    {
        private readonly IUserRepository _userRepository;

        public async Task<User?> Handle(DeleteUserConnectionCommand command, CancellationToken cancellationToken)
        {
            UserConnections? userConnections = await ConnectionsRepository
                .FindOrDefaultAsync(UserId, cancellationToken)!;

            User? user = null;

            if (userConnections == null)
                return null;

            userConnections.Connections.Remove(command.ConnectionId);
            if (userConnections.Connections.Count == 0)
            {
                await ConnectionsRepository.DeleteAsync(userConnections, cancellationToken);

                user = await _userRepository.FindAsync(UserId, cancellationToken);
                user.Status = UserStatus.Offline;

                await _userRepository.UpdateAsync(user, cancellationToken);
            }
            else
                await ConnectionsRepository.UpdateAsync(userConnections, cancellationToken);

            return user;
        }

        public DeleteUserConnectionCommandHandler(
          IHubContextProvider hubContextProvider,
          IAuthorizedUserProvider userProvider,
          IMapper mapper,
          IUserRepository userRepository,
          IConnectionsRepository connectionsRepository) :
          base(hubContextProvider, userProvider, mapper, connectionsRepository)
        {
            _userRepository = userRepository;
        }
    }
}
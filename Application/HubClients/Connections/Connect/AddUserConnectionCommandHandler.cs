using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Connections.Connect
{
    public class AddUserConnectionCommandHandler : HubHandler, IRequestHandler<AddUserConnectionCommand, User?>
    {
        private readonly IUserRepository _userRepository;


        public async Task<User?> Handle(AddUserConnectionCommand command, CancellationToken cancellationToken)
        {
            UserConnections? userConnections = await ConnectionsRepository
                .FindOrDefaultAsync(UserId, cancellationToken);
            User? user = null;

            if (userConnections == null)
            {
                userConnections = new UserConnections
                { UserId = UserId, Connections = [command.ConnectionId] };

                await ConnectionsRepository.AddAsync(userConnections, cancellationToken);

                user = await _userRepository.FindAsync(UserId, cancellationToken);

                user.Status = UserStatus.Online;

                await _userRepository.UpdateAsync(user, cancellationToken);
            }
            else
            {
                userConnections.Connections.Add(command.ConnectionId);
                await ConnectionsRepository.UpdateAsync(userConnections, cancellationToken);
            }

            return user;
        }
        public AddUserConnectionCommandHandler(IHubContextProvider hubContextProvider,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IUserRepository userRepository,
            IConnectionsRepository connectionsRepository)
            : base(hubContextProvider, userProvider, mapper, connectionsRepository)
        {
            _userRepository = userRepository;
        }
    }
}
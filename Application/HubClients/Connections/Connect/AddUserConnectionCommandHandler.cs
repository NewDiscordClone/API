using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.Connections.Connect
{
    public class AddUserConnectionCommandHandler : HubRequestHandlerBase, IRequestHandler<AddUserConnectionCommand, User?>
    {
        private readonly IRepository<User, Guid> _userRepository;
        public AddUserConnectionCommandHandler(
            IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IRepository<User, Guid> userRepository)
            : base(hubContextProvider, context, userProvider, mapper)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> Handle(AddUserConnectionCommand command, CancellationToken cancellationToken)
        {
            UserConnections? userConnections = await Context.UserConnections.FindOrDefaultAsync(UserId, cancellationToken);
            User? user = null;
            if (userConnections == null)
            {
                userConnections = new UserConnections
                { UserId = UserId, Connections = new HashSet<string>() { command.ConnectionId } };

                await Context.UserConnections.AddAsync(userConnections, cancellationToken);

                user = await _userRepository.FindAsync(UserId, cancellationToken);

                user.Status = UserStatus.Online;

                await _userRepository.UpdateAsync(user, cancellationToken);
            }
            else
            {
                userConnections.Connections.Add(command.ConnectionId);
                await Context.UserConnections.UpdateAsync(userConnections, cancellationToken);
            }

            return user;
        }
    }
}
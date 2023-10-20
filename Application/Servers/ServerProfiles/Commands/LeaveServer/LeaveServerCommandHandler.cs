using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.LeaveServer
{
    public class LeaveServerCommandHandler : RequestHandlerBase, IRequestHandler<LeaveServerCommand, ServerProfile>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public LeaveServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IServerProfileRepository serverProfileRepository)
            : base(context, userProvider)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task<ServerProfile> Handle(LeaveServerCommand command, CancellationToken cancellationToken)
        {
            Server server = await Context.Servers.FindAsync(command.ServerId, cancellationToken);

            ServerProfile serverProfile = await _serverProfileRepository.FindOrDefaultAsync(command.ProfileId, cancellationToken)
                ?? throw new EntityNotFoundException($"Server {command.ServerId} does not contains profile {command.ProfileId}");

            if (_serverProfileRepository.IsServerOwner(serverProfile.Id))
                throw new NoPermissionsException("Server's owner cant be removed from server");

            await _serverProfileRepository.DeleteAsync(serverProfile, cancellationToken);

            server.Profiles.Remove(serverProfile.Id);
            await Context.Servers.UpdateAsync(server, cancellationToken);

            return serverProfile;
        }
    }
}
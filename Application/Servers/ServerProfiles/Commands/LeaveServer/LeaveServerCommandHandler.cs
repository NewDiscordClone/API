using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.LeaveServer
{
    public class LeaveServerCommandHandler(IServerProfileRepository serverProfileRepository,
        IServerRepository serverRepository) : IRequestHandler<LeaveServerCommand, ServerProfile>
    {
        private readonly IServerProfileRepository _serverProfileRepository = serverProfileRepository;
        private readonly IServerRepository _serverRepository = serverRepository;

        public async Task<ServerProfile> Handle(LeaveServerCommand command, CancellationToken cancellationToken)
        {
            Server server = await _serverRepository.FindAsync(command.ServerId, cancellationToken);

            ServerProfile serverProfile = await _serverProfileRepository.FindOrDefaultAsync(command.ProfileId, cancellationToken)
                ?? throw new EntityNotFoundException($"Server {command.ServerId} does not contains profile {command.ProfileId}");

            if (_serverProfileRepository.IsServerOwner(serverProfile.Id))
                throw new NoPermissionsException("Server's owner cant be removed from server");

            await _serverProfileRepository.DeleteAsync(serverProfile, cancellationToken);

            server.Profiles.Remove(serverProfile.Id);
            await _serverRepository.UpdateAsync(server, cancellationToken);

            return serverProfile;
        }
    }
}
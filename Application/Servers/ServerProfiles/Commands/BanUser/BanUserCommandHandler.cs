using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.BanUser
{
    public class BanUserCommandHandler : RequestHandler, IRequestHandler<BanUserCommand, ServerProfile>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IServerRepository _serverRepository;

        public async Task<ServerProfile> Handle(BanUserCommand command, CancellationToken cancellationToken)
        {
            Server server = await _serverRepository.FindAsync(command.ServerId, cancellationToken);

            if (!server.Profiles.Contains(command.ProfileId))
                throw new InvalidOperationException($"Server {command.ServerId} does not contains profile {command.ProfileId}");

            if (_serverProfileRepository.IsServerOwner(command.ProfileId))
                throw new NoPermissionsException("Server's owner cant be banned from server");

            ServerProfile profileToRemove = await _serverProfileRepository.FindAsync(command.ProfileId, cancellationToken);
            await _serverProfileRepository.DeleteAsync(profileToRemove, cancellationToken);

            server.Profiles.Remove(profileToRemove.Id);
            server.BannedUsers.Add(profileToRemove.UserId);
            await _serverRepository.UpdateAsync(server, cancellationToken);

            return profileToRemove;
        }

        public BanUserCommandHandler(IAuthorizedUserProvider userProvider,
            IServerProfileRepository serverProfileRepository,
            IServerRepository serverRepository) : base(userProvider)
        {
            _serverProfileRepository = serverProfileRepository;
            _serverRepository = serverRepository;
        }
    }
}
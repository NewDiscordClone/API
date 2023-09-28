using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileDisplayName
{
    public class ChangeServerProfileDisplayNameCommandHandler : IRequestHandler<ChangeServerProfileDisplayNameCommand>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public ChangeServerProfileDisplayNameCommandHandler(IServerProfileRepository serverProfileRepository)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task Handle(ChangeServerProfileDisplayNameCommand command, CancellationToken cancellationToken)
        {
            ServerProfile serverProfile = await _serverProfileRepository.FindAsync(command.ProfileId, cancellationToken);

            serverProfile.DisplayName = command.NewDisplayName;
            await _serverProfileRepository.UpdateAsync(serverProfile);
        }
    }
}
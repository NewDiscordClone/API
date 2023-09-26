using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileRoles
{
    public class UpdateServerProfileRolesCommandHandler : IRequestHandler<UpdateServerProfileRolesCommand>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public UpdateServerProfileRolesCommandHandler(IServerProfileRepository serverProfileRepository)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task Handle(UpdateServerProfileRolesCommand command, CancellationToken cancellationToken)
        {
            ServerProfile profile = await _serverProfileRepository.FindAsync(command.ProfileId, cancellationToken);

            List<Guid> currentRoles = await _serverProfileRepository.GetRolesIdsAsync(profile.Id, cancellationToken);
            List<Guid> newRoles = command.Roles;

            List<Guid> rolesToAdd = newRoles.Except(currentRoles).ToList();
            List<Guid> rolesToRemove = currentRoles.Except(newRoles).ToList();

            if (rolesToRemove.Count > 0)
                await _serverProfileRepository.RemoveRolesAsync(profile.Id, rolesToRemove.ToArray());

            if (rolesToAdd.Count > 0)
                await _serverProfileRepository.AddRolesAsync(profile.Id, rolesToAdd.ToArray());
        }
    }
}
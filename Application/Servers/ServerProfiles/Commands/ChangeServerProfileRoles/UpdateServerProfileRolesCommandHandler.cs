using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces.Repositories;

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
            Guid profileId = command.ProfileId;

            List<Guid> currentRoles = await _serverProfileRepository.GetRolesIdsAsync(profileId, cancellationToken);
            List<Guid> newRoles = command.Roles;

            List<Guid> rolesToAdd = newRoles.Except(currentRoles).ToList();

            List<Guid> rolesToRemove = currentRoles
                .Except(newRoles)
                .Except(Constants.Roles.DefaultRoleIds)
                .ToList();

            if (rolesToRemove.Count > 0)
                await _serverProfileRepository.RemoveRolesAsync(profileId, rolesToRemove.ToArray());

            if (rolesToAdd.Count > 0)
                await _serverProfileRepository.AddRolesAsync(profileId, rolesToAdd.ToArray());
        }
    }
}
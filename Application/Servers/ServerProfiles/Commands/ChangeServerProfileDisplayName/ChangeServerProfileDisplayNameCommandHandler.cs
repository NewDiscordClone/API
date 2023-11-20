using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileDisplayName
{
    public class ChangeServerProfileDisplayNameCommandHandler : RequestHandlerBase,
        IRequestHandler<ChangeServerProfileDisplayNameCommand, ServerProfile>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public ChangeServerProfileDisplayNameCommandHandler(IServerProfileRepository serverProfileRepository, IAuthorizedUserProvider userProvider)
            : base(userProvider)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task<ServerProfile> Handle(ChangeServerProfileDisplayNameCommand command, CancellationToken cancellationToken)
        {
            ServerProfile serverProfile = await _serverProfileRepository
                .FindOrDefaultAsync(command.ProfileId, cancellationToken, true)
                ?? throw new EntityNotFoundException(command.ProfileId);

            bool hasPermission = false;

            if (serverProfile.UserId == UserId &&
                UserProvider.HasClaims(serverProfile, Constants.Claims.ChangeServerName))
            {
                hasPermission = true;
            }
            else
            {
                ServerProfile? currentUserProfile = await _serverProfileRepository
                    .FindUserProfileOnServerAsync(serverProfile.ServerId, UserId, cancellationToken);

                currentUserProfile = await _serverProfileRepository
                .FindOrDefaultAsync(currentUserProfile!.Id, cancellationToken, true);

                if (currentUserProfile is not null && (UserProvider.HasClaims(currentUserProfile,
                    Constants.Claims.ChangeSomeoneServerName)
                    || UserProvider.IsAdmin(currentUserProfile)))
                {
                    hasPermission = true;
                }
            }

            if (!hasPermission)
                throw new NoPermissionsException();


            serverProfile.DisplayName = command.NewDisplayName;
            await _serverProfileRepository.UpdateAsync(serverProfile, cancellationToken);

            return serverProfile;
        }
    }
}
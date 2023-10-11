using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileDisplayName
{
    public class ChangeServerProfileDisplayNameCommandHandler : RequestHandlerBase, IRequestHandler<ChangeServerProfileDisplayNameCommand>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public ChangeServerProfileDisplayNameCommandHandler(IServerProfileRepository serverProfileRepository, IAuthorizedUserProvider userProvider)
            : base(userProvider)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task Handle(ChangeServerProfileDisplayNameCommand command, CancellationToken cancellationToken)
        {
            ServerProfile serverProfile = await _serverProfileRepository.FindAsync(command.ProfileId, cancellationToken);

            bool hasPermission = false;

            if (serverProfile.UserId == UserId &&
                UserProvider.HasClaims(serverProfile, Constants.Claims.ChangeServerName))
            {
                hasPermission = true;
            }
            else if (UserProvider.HasClaims(serverProfile, Constants.Claims.ChangeSomeoneServerName))
            {
                hasPermission = true;
            }

            if (!hasPermission)
                throw new NoPermissionsException();


            serverProfile.DisplayName = command.NewDisplayName;
            await _serverProfileRepository.UpdateAsync(serverProfile);
        }
    }
}
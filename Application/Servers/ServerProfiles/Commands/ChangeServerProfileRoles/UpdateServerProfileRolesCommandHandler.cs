using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileRoles
{
    public class UpdateServerProfileRolesCommandHandler : IRequestHandler<UpdateServerProfileRolesCommand>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IAppDbContext _context;
        public UpdateServerProfileRolesCommandHandler(IServerProfileRepository serverProfileRepository)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task Handle(UpdateServerProfileRolesCommand command, CancellationToken cancellationToken)
        {
            ServerProfile profile = await _serverProfileRepository.FindAsync(command.ProfileId);

            profile.Roles.Clear();

            Role[] roles = await _context.Roles
                .Where(role => command.Roles.Contains(role.Id))
                .ToArrayAsync(cancellationToken);

            profile.Roles.AddRange(roles);

            await _serverProfileRepository.UpdateAsync(profile);
        }
    }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.GetServerProfiles
{
    public class ServerProfilesQueryHandler : IRequestHandler<ServerProfilesQuery, List<ServerProfile>>
    {
        private readonly IAppDbContext _context;

        public ServerProfilesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServerProfile>> Handle(ServerProfilesQuery request, CancellationToken cancellationToken)
        {
            List<ServerProfile> profiles = await _context.UserProfiles
                .OfType<ServerProfile>()
                .Where(x => x.ServerId == request.ServerId)
                .Include(x => x.Roles)
                .ToListAsync(cancellationToken);

            foreach (ServerProfile profile in profiles)
            {
                Role? mainRole = profile.Roles
                     .Where(role => role.Id != Constants.Roles.ServerMemberId && role.Id
                        != Constants.Roles.ServerOwnerId)
                     .MaxBy(role => role.Priority);

                profile.Roles.Clear();

                if (mainRole is not null)
                    profile.Roles.Add(mainRole);
            }

            return profiles;
        }
    }
}

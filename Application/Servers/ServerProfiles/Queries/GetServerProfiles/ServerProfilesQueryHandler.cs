using MediatR;
using Microsoft.EntityFrameworkCore;
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
                .ToListAsync(cancellationToken);

            return profiles;
        }
    }
}

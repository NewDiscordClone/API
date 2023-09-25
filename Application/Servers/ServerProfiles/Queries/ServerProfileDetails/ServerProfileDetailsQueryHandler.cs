using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.ServerProfileDetails
{
    public class ServerProfileDetailsQueryHandler : IRequestHandler<ServerProfileDetailsQuery, ServerProfile>
    {
        private readonly IAppDbContext _context;

        public ServerProfileDetailsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<ServerProfile> Handle(ServerProfileDetailsQuery request, CancellationToken cancellationToken)
        {
            ServerProfile? profile = await _context.UserProfiles
                .OfType<ServerProfile>()
                .Include(p => p.Roles)
                .ThenInclude(r => r.Claims)
                .SingleOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

            return profile ?? throw new EntityNotFoundException(request.ProfileId);
        }
    }
}

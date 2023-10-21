using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.ServerProfileDetails
{
    public class ServerProfileDetailsQueryHandler : IRequestHandler<ServerProfileDetailsQuery, ServerProfileViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRoleFactory _roleFactory;
        public ServerProfileDetailsQueryHandler(IAppDbContext context, IMapper mapper, IRoleFactory roleFactory)
        {
            _context = context;
            _mapper = mapper;
            _roleFactory = roleFactory;
        }

        public async Task<ServerProfileViewModel> Handle(ServerProfileDetailsQuery request, CancellationToken cancellationToken)
        {
            ServerProfile? profile = await _context.UserProfiles
                .OfType<ServerProfile>()
                .Include(p => p.Roles)
                .SingleOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

            User? user = await _context.Users.FindAsync(new object?[] { profile?.UserId },
                cancellationToken: cancellationToken);

            if (user == null || profile == null)
                throw new EntityNotFoundException(request.ProfileId);

            profile.Roles = profile.Roles.ExceptBy(Constants.Roles.DefaultRoleIds, role => role.Id).ToList();

            return _mapper.Map<ServerProfileViewModel>((profile, user));
        }
    }
}

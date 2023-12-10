using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;
using Sparkle.Domain.LookUps;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.GetServerProfiles
{
    public class ServerProfilesQueryHandler : IRequestHandler<ServerProfilesQuery, List<ServerProfileLookup>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public ServerProfilesQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ServerProfileLookup>> Handle(ServerProfilesQuery request, CancellationToken cancellationToken)
        {
            List<ServerProfile> profiles = await _context.UserProfiles
                .OfType<ServerProfile>()
                .Where(x => x.ServerId == request.ServerId)
                .Include(x => x.Roles)
                .ToListAsync(cancellationToken);

            Dictionary<ServerProfile, User> profileUserDictionary = await GetProfileUserDictionaryAsync(profiles, cancellationToken);

            List<ServerProfileLookup> lookups = profileUserDictionary.Select(kv =>
            {
                (ServerProfile profile, User user) = kv;

                Role? mainRole = profile.Roles
                .ExceptBy(Constants.Roles.DefaultRoleIds, role => role.Id)
                .MaxBy(role => role.Priority);

                profile.Roles = new List<Role>();
                if (mainRole != null)
                    profile.Roles.Add(mainRole);

                return _mapper.Map<ServerProfileLookup>((profile, user));
            }).ToList();

            return lookups;
        }

        private async Task<Dictionary<ServerProfile, User>> GetProfileUserDictionaryAsync(List<ServerProfile> profiles,
            CancellationToken cancellationToken)
        {
            List<Guid> userIds = profiles.Select(profile => profile.UserId).ToList();

            List<User> users = await _context.Users
                .Where(user => userIds.Contains(user.Id))
                .ToListAsync(cancellationToken);

            Dictionary<ServerProfile, User> profileUserDictionary = new();

            foreach (User user in users)
            {
                ServerProfile profile = profiles.Single(p => p.UserId == user.Id);
                profileUserDictionary.Add(profile, user);
            }

            return profileUserDictionary;
        }

    }
}

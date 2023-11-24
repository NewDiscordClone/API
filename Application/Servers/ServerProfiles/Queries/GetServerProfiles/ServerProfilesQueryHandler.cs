using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.GetServerProfiles
{
    public class ServerProfilesQueryHandler(IMapper mapper, IServerProfileRepository repository, IUserRepository userRepository) :
        RequestHandlerBase(mapper), IRequestHandler<ServerProfilesQuery, List<ServerProfileLookup>>
    {
        private readonly IServerProfileRepository _repository = repository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<List<ServerProfileLookup>> Handle(ServerProfilesQuery request, CancellationToken cancellationToken)
        {
            List<ServerProfile> profiles = await _repository.ExecuteCustomQuery(profiles => profiles
                .Where(x => x.ServerId == request.ServerId)
                .Include(x => x.Roles))
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

                return Mapper.Map<ServerProfileLookup>((profile, user));
            }).ToList();

            return lookups;
        }

        private async Task<Dictionary<ServerProfile, User>> GetProfileUserDictionaryAsync(List<ServerProfile> profiles,
            CancellationToken cancellationToken)
        {
            List<Guid> userIds = profiles.Select(profile => profile.UserId).ToList();

            List<User> users = await _userRepository.ExecuteCustomQuery(users => users
                .Where(user => userIds.Contains(user.Id)))
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

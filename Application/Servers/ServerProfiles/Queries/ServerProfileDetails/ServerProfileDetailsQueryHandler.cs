using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.ServerProfileDetails
{
    public class ServerProfileDetailsQueryHandler : RequestHandler, IRequestHandler<ServerProfileDetailsQuery, ServerProfileViewModel>
    {
        private readonly IRoleFactory _roleFactory;
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IUserRepository _userRepository;

        public async Task<ServerProfileViewModel> Handle(ServerProfileDetailsQuery query, CancellationToken cancellationToken)
        {
            ServerProfile profile = await _serverProfileRepository
                .FindOrDefaultAsync(query.ProfileId, cancellationToken, true)
                ?? throw new EntityNotFoundException(query.ProfileId);

            User user = await _userRepository.FindAsync(profile.UserId, cancellationToken);

            profile.Roles = profile.Roles.ExceptBy(Constants.Roles.DefaultRoleIds, role => role.Id).ToList();

            return Mapper.Map<ServerProfileViewModel>((profile, user));
        }

        public ServerProfileDetailsQueryHandler(IMapper mapper,
            IRoleFactory roleFactory,
            IUserRepository userRepository,
            IServerProfileRepository serverProfileRepository) : base(mapper)
        {
            _roleFactory = roleFactory;
            _userRepository = userRepository;
            _serverProfileRepository = serverProfileRepository;
        }
    }
}

using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Queries
{
    public class GetUserDetailsQueryHandler : RequestHandler, IRequestHandler<GetUserDetailsQuery, GetUserDetailsDto>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IUserRepository _userRepository;

        public async Task<GetUserDetailsDto> Handle(GetUserDetailsQuery query, CancellationToken cancellationToken)
        {
            User user = await _userRepository.FindAsync(query.UserId, cancellationToken);
            GetUserDetailsDto userDto = Mapper.Map<GetUserDetailsDto>(user);

            if (query.ServerId is not null)
            {
                ServerProfile? serverProfile = await _serverProfileRepository
                    .FindUserProfileOnServerAsync(query.ServerId, query.UserId, cancellationToken);

                if (serverProfile != null)
                {
                    serverProfile = await _serverProfileRepository.
                        FindOrDefaultAsync(serverProfile.Id, cancellationToken, true);

                    serverProfile!.Roles = serverProfile.Roles
                        .ExceptBy(Constants.Roles.DefaultRoleIds, role => role.Id).ToList();

                    userDto.ServerProfile = Mapper.Map<GetUserDetailsServerProfileDto>(serverProfile);
                }
            }
            return userDto;
        }

        public GetUserDetailsQueryHandler(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IServerProfileRepository serverProfileRepository,
            IUserRepository userRepository) : base(userProvider, mapper)
        {
            _serverProfileRepository = serverProfileRepository;
            _userRepository = userRepository;
        }
    }
}
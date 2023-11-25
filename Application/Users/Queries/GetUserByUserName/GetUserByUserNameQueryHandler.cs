using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Queries
{
    public class GetUserByUserNameQueryHandler : RequestHandler,
        IRequestHandler<GetUserByUserNameQuery, GetUserDetailsDto>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServerRepository _serverRepository;

        public async Task<GetUserDetailsDto> Handle(GetUserByUserNameQuery query, CancellationToken cancellationToken)
        {
            User user = await _userRepository.SingleAsync(user => user.UserName == query.UserName, cancellationToken);

            GetUserDetailsDto userDto = Mapper.Map<GetUserDetailsDto>(user);

            if (query.ServerId is not null)
            {
                Server server = await _serverRepository.FindAsync(query.ServerId, cancellationToken);
                ServerProfile? serverProfile = await _serverProfileRepository.SingleAsync(profile => profile.UserId == user.Id
                 && profile.ServerId == server.Id, cancellationToken);
                userDto.ServerProfile = Mapper.Map<GetUserDetailsServerProfileDto>(serverProfile);
            }
            return userDto;
        }

        public GetUserByUserNameQueryHandler(IMapper mapper,
            IServerProfileRepository serverProfileRepository,
            IUserRepository userRepository,
            IServerRepository serverRepository) : base(mapper)
        {
            _serverProfileRepository = serverProfileRepository;
            _userRepository = userRepository;
            _serverRepository = serverRepository;
        }
    }
}
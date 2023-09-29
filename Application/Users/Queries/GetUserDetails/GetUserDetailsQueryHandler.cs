using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Queries.GetUserDetails
{
    public class GetUserDetailsQueryHandler : RequestHandlerBase, IRequestHandler<GetUserDetailsQuery, GetUserDetailsDto>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public async Task<GetUserDetailsDto> Handle(GetUserDetailsQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            User user = await Context.SqlUsers.FindAsync(query.UserId, cancellationToken);
            GetUserDetailsDto userDto = Mapper.Map<GetUserDetailsDto>(user);

            if (query.ServerId is not null)
            {
                ServerProfile? serverProfile = await _serverProfileRepository
                    .FindUserProfileOnServerAsync(query.ServerId, query.UserId, cancellationToken);
                userDto.ServerProfile = Mapper.Map<GetUserDetailsServerProfileDto>(serverProfile);
            }
            return userDto;
        }

        public GetUserDetailsQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper, IServerProfileRepository serverProfileRepository) : base(context, userProvider, mapper)
        {
            _serverProfileRepository = serverProfileRepository;
        }
    }
}
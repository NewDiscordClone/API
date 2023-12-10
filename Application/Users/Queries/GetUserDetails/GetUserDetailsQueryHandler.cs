using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Queries
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

        public GetUserDetailsQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper, IServerProfileRepository serverProfileRepository) : base(context, userProvider, mapper)
        {
            _serverProfileRepository = serverProfileRepository;
        }
    }
}
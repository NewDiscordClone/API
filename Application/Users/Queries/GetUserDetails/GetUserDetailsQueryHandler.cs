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

            User user = await Context.SqlUsers.FindAsync(query.UserId);
            GetUserDetailsDto userDto = Mapper.Map<GetUserDetailsDto>(user);

            if (query.ServerId is not null)
            {
                Server server = await Context.Servers.FindAsync(query.ServerId);
                ServerProfile? serverProfile = await _serverProfileRepository.SingleAsync(profile => profile.UserId == user.Id);
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
using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Users.Queries.GetUserDetails;

namespace Sparkle.Application.Users.Queries
{
    public class GetUserByUserNameQueryHandler : RequestHandlerBase,
        IRequestHandler<GetUserByUserNameQuery, GetUserDetailsDto>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public GetUserByUserNameQueryHandler(IAppDbContext context, IMapper mapper, IServerProfileRepository serverProfileRepository) : base(context, mapper)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task<GetUserDetailsDto> Handle(GetUserByUserNameQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            User user = (await Context.SqlUsers
                            .FilterAsync(u => u.UserName == query.UserName))
                        .FirstOrDefault()
                        ??
                        throw new EntityNotFoundException($"There is no User with {query.UserName} user name", query.UserName);
            GetUserDetailsDto userDto = Mapper.Map<GetUserDetailsDto>(user);

            if (query.ServerId is not null)
            {
                Server server = await Context.Servers.FindAsync(query.ServerId);
                ServerProfile? serverProfile = await _serverProfileRepository.SingleAsync(profile => profile.UserId == user.Id);
                userDto.ServerProfile = Mapper.Map<GetUserDetailsServerProfileDto>(serverProfile);
            }
            return userDto;
        }
    }
}
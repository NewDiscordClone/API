using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GetUser
{
    public class GetUserDeatilsRequestHandler : RequestHandlerBase, IRequestHandler<GetUserDetailsRequest, GetUserDetailsDto>
    {

        public async Task<GetUserDetailsDto> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            User user = await Context.FindSqlByIdAsync<User>(request.UserId, cancellationToken);
            GetUserDetailsDto userDto = Mapper.Map<GetUserDetailsDto>(user);

            if (request.ServerId is not null)
            {
                var server = await Context.Servers.FindAsync(request.ServerId.Value);
                var serverProfile = server.ServerProfiles.FirstOrDefault(profile => profile.User.Id == request.UserId);
                userDto.ServerProfile = Mapper.Map<GetUserDetailsServerProfileDto>(serverProfile);
            }
            return userDto;
        }

        public GetUserDeatilsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
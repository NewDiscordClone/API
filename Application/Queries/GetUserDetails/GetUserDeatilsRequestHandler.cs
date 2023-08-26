using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GetUser
{
    public class GetUserDeatilsRequestHandler : RequestHandlerBase, IRequestHandler<GetUserDetailsRequest, GetUserDetailsDto>
    {

        public async Task<GetUserDetailsDto> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.Users
                  .FindAsync(new object[] { request.UserId }, cancellationToken)
                  ?? throw new NoSuchUserException();
            GetUserDetailsDto userDto = Mapper.Map<GetUserDetailsDto>(user);

            if (request.ServerId is not null)
            {
                var server = await Context.FindByIdAsync<Server>(request.ServerId.Value, cancellationToken);
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
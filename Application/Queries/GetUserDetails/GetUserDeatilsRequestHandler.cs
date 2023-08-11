using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GetUser
{
    public class GetUserDeatilsRequestHandler : IRequestHandler<GetUserDetailsRequest, GetUserDetailsDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetUserDeatilsRequestHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<GetUserDetailsDto> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            User user = await _appDbContext.Users
                  .FindAsync(new object[] { request.UserId }, cancellationToken)
                  ?? throw new NoSuchUserException();
            GetUserDetailsDto userDto = _mapper.Map<GetUserDetailsDto>(user);

            if (request.ServerId is not null)
            {
                ServerProfile? profile = _appDbContext.ServerProfiles
                    .Include(sp => sp.Server)
                    .Include(sp => sp.User)
                    .Include(sp => sp.Roles)
                    .FirstOrDefault(profile => profile.Server.Id == request.ServerId
                    && profile.User.Id == request.UserId);
                userDto.Profile = _mapper.Map<GetUserDetailsServerProfileDto>(profile);
            }
            return userDto;
        }
    }
}
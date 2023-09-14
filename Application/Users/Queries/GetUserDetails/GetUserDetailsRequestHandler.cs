﻿using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Queries.GetUserDetails
{
    public class GetUserDetailsRequestHandler : RequestHandlerBase, IRequestHandler<GetUserDetailsRequest, GetUserDetailsDto>
    {

        public async Task<GetUserDetailsDto> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            User user = await Context.SqlUsers.FindAsync(request.UserId);
            GetUserDetailsDto userDto = Mapper.Map<GetUserDetailsDto>(user);

            if (request.ServerId is not null)
            {
                Server server = await Context.Servers.FindAsync(request.ServerId);
                ServerProfile? serverProfile = server.ServerProfiles.FirstOrDefault(profile => profile.UserId == request.UserId);
                userDto.ServerProfile = Mapper.Map<GetUserDetailsServerProfileDto>(serverProfile);
            }
            return userDto;
        }

        public GetUserDetailsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
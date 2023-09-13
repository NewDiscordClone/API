﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Common.Servers.Queries.GetServerDetails
{
    public class GetServerDetailsRequestHandler : RequestHandlerBase,
        IRequestHandler<GetServerDetailsRequest, ServerDetailsDto>
    {
        public async Task<ServerDetailsDto> Handle(GetServerDetailsRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(request.ServerId);

            if (server.ServerProfiles.Find(sp => sp.UserId == UserId) == null)
                throw new NoPermissionsException("User are not a member of the Server");

            ServerDetailsDto res = Mapper.Map<ServerDetailsDto>(server);

            res.Channels = await Context.Channels.FilterAsync(c => c.ServerId == server.Id);
            return res;
        }

        public GetServerDetailsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
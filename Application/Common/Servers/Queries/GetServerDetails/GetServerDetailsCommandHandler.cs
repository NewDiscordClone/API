﻿using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Servers.Queries.GetServerDetails
{
    public class GetServerDetailsCommandHandler : RequestHandlerBase,
        IRequestHandler<GetServerDetailsCommand, ServerDetailsDto>
    {
        public async Task<ServerDetailsDto> Handle(GetServerDetailsCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(command.ServerId);

            if (server.ServerProfiles.Find(sp => sp.UserId == UserId) == null)
                throw new NoPermissionsException("User are not a member of the Server");

            ServerDetailsDto res = Mapper.Map<ServerDetailsDto>(server);

            res.Channels = await Context.Channels.FilterAsync(c => c.ServerId == server.Id);
            return res;
        }

        public GetServerDetailsCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
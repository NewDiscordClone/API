﻿using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Servers.Commands.CreateServer
{
    public class CreateServerRequestHandler : RequestHandlerBase, IRequestHandler<CreateServerRequest, string>
    {
        public async Task<string> Handle(CreateServerRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = new()
            {
                Title = request.Title,
                Image = request.Image,
                Owner = UserId,
                ServerProfiles = new List<ServerProfile>()
                {
                    new ServerProfile()
                    {
                        UserId = UserId
                    }
                }
            };

            server = await Context.Servers.AddAsync(server);
            return server.Id;
        }

        public CreateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
            : base(context, userProvider, mapper)
        {
        }
    }
}
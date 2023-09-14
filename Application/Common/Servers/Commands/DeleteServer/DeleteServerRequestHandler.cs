﻿using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Servers.Commands.DeleteServer
{
    public class DeleteServerRequestHandler : RequestHandlerBase, IRequestHandler<DeleteServerCommand, Server>
    {

        public async Task<Server> Handle(DeleteServerCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(request.ServerId);

            if (UserId != server.Owner)
                throw new NoPermissionsException("You are not the owner of the server");

            await Context.Servers.DeleteAsync(server);
            await Context.Channels.DeleteManyAsync(c => c.ServerId == request.ServerId);

            return server;
        }

        public DeleteServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
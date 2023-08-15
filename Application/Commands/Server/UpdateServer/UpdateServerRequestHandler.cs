﻿using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
namespace Application.Commands.Server.UpdateServer
{
    public class UpdateServerRequestHandler : RequestHandlerBase, IRequestHandler<UpdateServerRequest>
    {
        public async Task Handle(UpdateServerRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);
            Models.Server server = await Context.FindByIdAsync<Models.Server>
                (request.ServerId, cancellationToken);
            
            if (user.Id != server.Owner.Id)
                throw new NoPermissionsException("You are not the owner of the server");
            
            server.Title = request.Title ?? server.Title;
            server.Image = request.Image ?? server.Image;

            Context.Servers.Update(server);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public UpdateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
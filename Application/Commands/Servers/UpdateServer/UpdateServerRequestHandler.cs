using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.Servers.UpdateServer
{
    public class UpdateServerRequestHandler : RequestHandlerBase, IRequestHandler<UpdateServerRequest>
    {
        public async Task Handle(UpdateServerRequest request, CancellationToken cancellationToken)
        {
            Server server = await Context.FindByIdAsync<Server>
                (request.ServerId, cancellationToken);

            if (UserId != server.Owner.Id)
                throw new NoPermissionsException("You are not the owner of the server");

            UpdateDefinition<Server> toUpdate;
            UpdateDefinition<Server> updateTitle = Builders<Server>.Update.Set(s => s.Title, request.Title);
            UpdateDefinition<Server> updateImage = Builders<Server>.Update.Set(s => s.Image, request.Image);

            if (request.Image != null && server.Image != null)
                await Context.CheckRemoveMedia(server.Image[(server.Image.LastIndexOf('/') - 1)..], cancellationToken);

            if (request is { Title: { }, Image: { } })
                toUpdate = Builders<Server>.Update.Combine(updateTitle, updateImage);
            else if (request.Image != null)
                toUpdate = updateImage;
            else if (request.Title != null)
                toUpdate = updateTitle;
            else throw new Exception("Your request isn't changing anything");

            await Context.Servers.UpdateOneAsync(Context.GetIdFilter<Server>(request.ServerId), toUpdate, null, cancellationToken);
        }

        public UpdateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
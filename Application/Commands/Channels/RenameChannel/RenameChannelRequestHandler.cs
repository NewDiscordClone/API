using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.Channels.RenameChannel
{
    public class RenameChannelRequestHandler :RequestHandlerBase, IRequestHandler<RenameChannelRequest>
    {
        public async Task Handle(RenameChannelRequest request, CancellationToken cancellationToken)
        {
            Channel chat =
                await Context.FindByIdAsync<Channel>(request.ChatId, cancellationToken);
            
            //TODO: Перевірити що у юзера є відповідні права
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            
            
            await Context.Channels.UpdateOneAsync(
                Context.GetIdFilter<Channel>(chat.Id),
                Builders<Channel>.Update.Set(c => c.Title, request.NewTitle),
                null,
                cancellationToken);
        }

        public RenameChannelRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.RenamePrivateChat
{
    public class RenamePrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<RenamePrivateChatRequest>
    {
        public async Task Handle(RenamePrivateChatRequest request, CancellationToken cancellationToken)
        {
            PrivateChat chat =
                await Context.FindByIdAsync<PrivateChat>(request.ChatId, cancellationToken);
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");

           await Context.PrivateChats.UpdateOneAsync(
                Context.GetIdFilter<PrivateChat>(chat.Id),
                Builders<PrivateChat>.Update.Set(c => c.Title, request.NewTitle),
                null,
                cancellationToken
            );
        }

        public RenamePrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}

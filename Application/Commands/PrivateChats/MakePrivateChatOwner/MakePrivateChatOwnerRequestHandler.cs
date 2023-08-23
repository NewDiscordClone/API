using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.MakePrivateChatOwner
{
    public class MakePrivateChatOwnerRequestHandler : RequestHandlerBase, IRequestHandler<MakePrivateChatOwnerRequest>
    {
        public async Task Handle(MakePrivateChatOwnerRequest request, CancellationToken cancellationToken)
        {
            PrivateChat chat =
                await Context.FindByIdAsync<Models.PrivateChat>(request.ChatId, cancellationToken);
            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("User is not an owner of the chat");
            if (!chat.Users.Any(u => u.Id == request.MemberId))
                throw new NoSuchUserException("User in not a member of the chat");


            await Context.PrivateChats.UpdateOneAsync(
                Context.GetIdFilter<PrivateChat>(chat.Id),
                Builders<PrivateChat>.Update.Set(p => p.OwnerId, request.MemberId),
                null,
                cancellationToken);
        }

        public MakePrivateChatOwnerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
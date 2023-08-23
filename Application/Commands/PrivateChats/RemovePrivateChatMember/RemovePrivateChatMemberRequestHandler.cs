using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.RemovePrivateChatMember
{
    public class RemovePrivateChatMemberRequestHandler : RequestHandlerBase,
        IRequestHandler<RemovePrivateChatMemberRequest>
    {
        public async Task Handle(RemovePrivateChatMemberRequest request, CancellationToken cancellationToken)
        {
            PrivateChat chat =
                await Context.FindByIdAsync<PrivateChat>(request.ChatId, cancellationToken);

            if (!chat.Users.Any(u => u.Id == request.MemberId))
                throw new NoSuchUserException();
            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("You are not an owner of the chat");
            if (UserId == request.MemberId)
                throw new Exception("You can't remove yourself");

            //User member = await Context.FindSqlByIdAsync<User>(request.MemberId, cancellationToken);
            await Context.Chats.UpdateOneAsync(
                Context.GetIdFilter<Chat>(chat.Id),
                Builders<Chat>.Update.PullFilter(c => c.Users,
                    Builders<UserLookUp>.Filter.Eq(u => u.Id, request.MemberId)),
                null,
                cancellationToken
            );
        }

        public RemovePrivateChatMemberRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}
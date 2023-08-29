using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.RemovePrivateChatMember
{
    public class RemovePrivateChatMemberRequestHandler : RequestHandlerBase,
        IRequestHandler<RemovePrivateChatMemberRequest, PrivateChat>
    {
        public async Task<PrivateChat> Handle(RemovePrivateChatMemberRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            PrivateChat chat = await Context.PrivateChats.FindAsync(request.ChatId);

            if (!chat.Users.Any(u => u.Id == request.MemberId))
                throw new NoSuchUserException();
            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("You are not an owner of the chat");
            if (UserId == request.MemberId)
                throw new Exception("You can't remove yourself");

            //User member = await Context.FindSqlByIdAsync<User>(request.MemberId, cancellationToken);
            chat.Users.Remove(chat.Users.Find(u => u.Id == request.MemberId) ?? throw new NoSuchUserException());

            return await Context.PrivateChats.UpdateAsync(chat);
        }

        public RemovePrivateChatMemberRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}
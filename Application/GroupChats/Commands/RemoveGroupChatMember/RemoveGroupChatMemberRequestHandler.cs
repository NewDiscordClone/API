using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.RemoveGroupChatMember
{
    public class RemoveGroupChatMemberRequestHandler : RequestHandlerBase,
        IRequestHandler<RemoveGroupChatMemberRequest>
    {
        public async Task Handle(RemoveGroupChatMemberRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(request.ChatId);
            if (pchat is not GroupChat chat)
                throw new Exception("This is not group chat");

            if (!chat.Users.Any(u => u == request.MemberId))
                throw new NoSuchUserException();
            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("You are not an owner of the chat");
            if (UserId == request.MemberId)
                throw new Exception("You can't remove yourself");

            //User member = await Context.FindSqlByIdAsync<User>(request.MemberId, cancellationToken);
            if (!chat.Users.Contains(request.MemberId))
                throw new NoSuchUserException();
            chat.Users.Remove(chat.Users.Find(u => u == request.MemberId));

            await Context.GroupChats.UpdateAsync(chat);
        }

        public RemoveGroupChatMemberRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}
using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.RemoveGroupChatMember
{
    public class RemoveGroupChatMemberCommandHandler : RequestHandlerBase,
        IRequestHandler<RemoveGroupChatMemberCommand>
    {
        public async Task Handle(RemoveGroupChatMemberCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(command.ChatId);
            if (pchat is not GroupChat chat)
                throw new Exception("This is not group chat");

            if (!chat.Users.Any(u => u == command.MemberId))
                throw new NoSuchUserException();
            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("You are not an owner of the chat");
            if (UserId == command.MemberId)
                throw new Exception("You can't remove yourself");

            //User member = await Context.FindSqlByIdAsync<User>(request.MemberId, cancellationToken);
            if (!chat.Users.Contains(command.MemberId))
                throw new NoSuchUserException();
            chat.Users.Remove(chat.Users.Find(u => u == command.MemberId));

            await Context.GroupChats.UpdateAsync(chat);
        }

        public RemoveGroupChatMemberCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}
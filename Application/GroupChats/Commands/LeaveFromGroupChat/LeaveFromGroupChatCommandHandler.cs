using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.LeaveFromGroupChat
{
    public class LeaveFromGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<LeaveFromGroupChatCommand>
    {
        public async Task Handle(LeaveFromGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(command.ChatId);
            if (pchat is not GroupChat chat)
                throw new Exception("This is not group chat");

            if (!chat.Users.Any(u => u == UserId))
                throw new NoSuchUserException("User is not a member of the chat");

            if (chat.Users.Count <= 1)
            {
                await Context.Chats.DeleteAsync(chat);
            }
            else
            {
                if (!chat.Users.Contains(UserId))
                    throw new NoSuchUserException();
                chat.Users.Remove(chat.Users.Find(u => u == UserId));
                if (chat.OwnerId == UserId)
                    chat.OwnerId = chat.Users.First(u => u != UserId);

                await Context.GroupChats.UpdateAsync(chat);
            }
        }

        public LeaveFromGroupChatCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
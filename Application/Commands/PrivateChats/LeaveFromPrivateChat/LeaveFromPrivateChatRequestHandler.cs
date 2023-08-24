using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.PrivateChats.LeaveFromPrivateChat
{
    public class LeaveFromPrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<LeaveFromPrivateChatRequest>
    {
        public async Task Handle(LeaveFromPrivateChatRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            Models.PrivateChat chat =
                await Context.FindByIdAsync<Models.PrivateChat>(request.ChatId, cancellationToken, "Users");
            if (chat.Users.Find(u => u.Id == user.Id) == null)
                throw new NoSuchUserException("User is not a member of the chat");
            chat.Users.Remove(user);
            if (chat.Users.Count <= 1)
            {
                Context.Chats.Remove(chat);
            }
            else if (chat.OwnerId == UserId)
                chat.OwnerId = chat.Users.First().Id;
            await Context.SaveChangesAsync(cancellationToken);
        }

        public LeaveFromPrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
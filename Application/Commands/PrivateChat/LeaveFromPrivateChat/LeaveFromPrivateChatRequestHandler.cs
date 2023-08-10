using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.PrivateChat.LeaveFromPrivateChat
{
    public class LeaveFromPrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<LeaveFromPrivateChatRequest>
    {

        public async Task Handle(LeaveFromPrivateChatRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            Models.PrivateChat chat =
                await Context.FindByIdAsync<Models.PrivateChat>(request.ChatId, cancellationToken);
            if (chat.Users.Find(u => u.Id == user.Id) == null)
                throw new NoSuchUserException("User is not a member of the chat");
            chat.Users.Remove(user);
            await Context.SaveChangesAsync(cancellationToken);
            if (chat.Owner.Id == UserId)
                chat.Owner = chat.Users.First();
            await Context.SaveChangesAsync(cancellationToken);
        }

        public LeaveFromPrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
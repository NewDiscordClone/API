using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.PrivateChat.RemovePrivateChatMember
{
    public class RemovePrivateChatMemberRequestHandler : RequestHandlerBase, IRequestHandler<RemovePrivateChatMemberRequest>
    {
        
        public async Task Handle(RemovePrivateChatMemberRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            Models.PrivateChat chat =
                await Context.FindByIdAsync<Models.PrivateChat>(request.ChatId, cancellationToken);
            if (chat.Owner.Id != user.Id)
                throw new NoPermissionsException("User is not an owner of the chat");

            User member = await Context.FindByIdAsync<User>(request.MemberId, cancellationToken);
            if (!chat.Users.Remove(member))
                throw new NoSuchUserException();
            await Context.SaveChangesAsync(cancellationToken);
        }

        public RemovePrivateChatMemberRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
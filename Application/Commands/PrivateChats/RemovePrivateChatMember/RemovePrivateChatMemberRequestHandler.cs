using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.PrivateChats.RemovePrivateChatMember
{
    public class RemovePrivateChatMemberRequestHandler : RequestHandlerBase, IRequestHandler<RemovePrivateChatMemberRequest>
    {
        
        public async Task Handle(RemovePrivateChatMemberRequest request, CancellationToken cancellationToken)
        {
            Models.PrivateChat chat =
                await Context.FindByIdAsync<Models.PrivateChat>(request.ChatId, cancellationToken, "Users");
            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("You are not an owner of the chat");
            if (UserId == request.MemberId)
                throw new Exception("You can't remove yourself");

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
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.RemovePrivateChatMember
{
    public class RemovePrivateChatMemberRequestHandler : RequestHandlerBase,
        IRequestHandler<RemovePrivateChatMemberRequest>
    {
        public async Task Handle(RemovePrivateChatMemberRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            PrivateChat pchat = await Context.PrivateChats.FindAsync(request.ChatId);
            if(pchat is not GroupChat chat) throw new Exception("This is not group chat");

            if (!chat.Users.Any(u => u.Id == request.MemberId))
                throw new NoSuchUserException();
            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("You are not an owner of the chat");
            if (UserId == request.MemberId)
                throw new Exception("You can't remove yourself");

            //User member = await Context.FindSqlByIdAsync<User>(request.MemberId, cancellationToken);
            chat.Users.Remove(chat.Users.Find(u => u.Id == request.MemberId) ?? throw new NoSuchUserException());

            await Context.PrivateChats.UpdateAsync(chat);
        }

        public RemovePrivateChatMemberRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}
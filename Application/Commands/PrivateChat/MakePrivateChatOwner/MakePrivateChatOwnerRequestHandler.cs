using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.PrivateChat.MakePrivateChatOwner
{
    public class MakePrivateChatOwnerRequestHandler : RequestHandlerBase, IRequestHandler<MakePrivateChatOwnerRequest>
    {


        public async Task Handle(MakePrivateChatOwnerRequest request, CancellationToken cancellationToken)
        {
            Models.PrivateChat chat =
                await Context.FindByIdAsync<Models.PrivateChat>(request.ChatId, cancellationToken, "Users", "Owner");
            if (chat.Owner.Id != UserId)
                throw new NoPermissionsException("User is not an owner of the chat");

            User member = await Context.FindByIdAsync<User>(request.MemberId, cancellationToken);
            if (chat.Users.Find(u => u.Id == member.Id) == null)
                throw new NoSuchUserException("User in not a member of the chat");
            chat.Owner = member;
            await Context.SaveChangesAsync(cancellationToken);
        }

        public MakePrivateChatOwnerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
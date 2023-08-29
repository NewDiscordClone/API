using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.MakePrivateChatOwner
{
    public class MakePrivateChatOwnerRequestHandler : RequestHandlerBase, IRequestHandler<MakePrivateChatOwnerRequest>
    {
        public async Task Handle(MakePrivateChatOwnerRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            
            PrivateChat chat = await Context.PrivateChats.FindAsync(request.ChatId);
            
            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("User is not an owner of the chat");
            if (!chat.Users.Any(u => u.Id == request.MemberId))
                throw new NoSuchUserException("User in not a member of the chat");

            chat.OwnerId = request.MemberId;
            await Context.PrivateChats.UpdateAsync(chat);
        }

        public MakePrivateChatOwnerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
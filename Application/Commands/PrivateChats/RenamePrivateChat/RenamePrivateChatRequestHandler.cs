using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.RenamePrivateChat
{
    public class RenamePrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<RenamePrivateChatRequest, PrivateChat>
    {
        public async Task<PrivateChat> Handle(RenamePrivateChatRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            
            PrivateChat chat = await Context.PrivateChats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");

            chat.Title = request.NewTitle;
            
            return await Context.PrivateChats.UpdateAsync(chat);
        }

        public RenamePrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}

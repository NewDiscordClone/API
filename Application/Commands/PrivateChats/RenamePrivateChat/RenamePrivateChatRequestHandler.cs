using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.RenamePrivateChat
{
    public class RenamePrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<RenamePrivateChatRequest>
    {
        public async Task Handle(RenamePrivateChatRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            PrivateChat chat = await Context.PrivateChats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");

            chat.Title = request.NewTitle;

            await Context.PrivateChats.UpdateAsync(chat);
        }

        public RenamePrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}

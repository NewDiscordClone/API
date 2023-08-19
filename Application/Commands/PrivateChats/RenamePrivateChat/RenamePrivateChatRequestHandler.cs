using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.PrivateChats.RenamePrivateChat
{
    public class RenamePrivateChatRequestHandler :RequestHandlerBase, IRequestHandler<RenamePrivateChatRequest>
    {

        public async Task Handle(RenamePrivateChatRequest request, CancellationToken cancellationToken)
        {
            Models.PrivateChat chat =
                await Context.FindByIdAsync<Models.PrivateChat>(request.ChatId, cancellationToken);
            if (chat.Users.Find(u => u.Id == UserId) == null)
                throw new NoPermissionsException("User is not a member of the chat");
            chat.Title = request.NewTitle;

            await Context.SaveChangesAsync(cancellationToken);
        }

        public RenamePrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.PrivateChat.ChangePrivateChatImage
{
    public class RenamePrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<ChangePrivateChatImageRequest>
    {

        public async Task Handle(ChangePrivateChatImageRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken)
                        ?? throw new NoSuchUserException();

            Models.PrivateChat chat =
                await Context.FindByIdAsync<Models.PrivateChat>(request.ChatId, cancellationToken);
            if (chat.Users.Find(u => u.Id == user.Id) == null)
                throw new NoPermissionsException("User is not a member of the chat");
            chat.Image = request.NewImage;

            await Context.SaveChangesAsync(cancellationToken);
        }

        public RenamePrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
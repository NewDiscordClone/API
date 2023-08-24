using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.PrivateChats.ChangePrivateChatImage
{
    public class ChangePrivateChatImageRequestHandler : RequestHandlerBase, IRequestHandler<ChangePrivateChatImageRequest>
    {
        public async Task Handle(ChangePrivateChatImageRequest request, CancellationToken cancellationToken)
        {
            PrivateChat chat =
                await Context.FindByIdAsync<PrivateChat>(request.ChatId, cancellationToken, "Users");
            if (chat.Users.Find(u => u.Id == UserId) == null)
                throw new NoPermissionsException("User is not a member of the chat");
            chat.Image = request.NewImage;

            await Context.SaveChangesAsync(cancellationToken);
        }

        public ChangePrivateChatImageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
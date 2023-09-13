using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using MediatR;
using MongoDB.Driver;

namespace Application.GroupChats.Commands.ChangeGroupChatImage
{
    public class ChangeGroupChatImageRequestHandler : RequestHandlerBase, IRequestHandler<ChangeGroupChatImageRequest>
    {
        public async Task Handle(ChangeGroupChatImageRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            string? oldImage = chat.Image;

            chat.Image = request.NewImage;

            chat = await Context.GroupChats.UpdateAsync(chat);

            if (oldImage != null)
                await Context.CheckRemoveMedia(oldImage[(oldImage.LastIndexOf('/') - 1)..]);

        }

        public ChangeGroupChatImageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
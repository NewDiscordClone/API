using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.ChangeGroupChatImage
{
    public class ChangeGroupChatImageCommandHandler : RequestHandlerBase, IRequestHandler<ChangeGroupChatImageCommand>
    {
        public async Task Handle(ChangeGroupChatImageCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(command.ChatId);
            if (!chat.Profiles.Any(p => p.UserId == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            string? oldImage = chat.Image;

            chat.Image = command.NewImage;

            chat = await Context.GroupChats.UpdateAsync(chat);

            if (oldImage != null)
                await Context.CheckRemoveMedia(oldImage[(oldImage.LastIndexOf('/') - 1)..]);

        }

        public ChangeGroupChatImageCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
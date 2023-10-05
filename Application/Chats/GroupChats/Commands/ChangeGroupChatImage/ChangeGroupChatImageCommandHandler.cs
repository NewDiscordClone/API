using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.GroupChats.Commands.ChangeGroupChatImage
{
    public class ChangeGroupChatImageCommandHandler : RequestHandlerBase, IRequestHandler<ChangeGroupChatImageCommand>
    {
        public async Task Handle(ChangeGroupChatImageCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(command.ChatId);

            string? oldImage = chat.Image;
            chat.Image = command.NewImage;

            await Context.GroupChats.UpdateAsync(chat);

            if (oldImage != null)
                await Context.CheckRemoveMedia(oldImage[(oldImage.LastIndexOf('/') - 1)..]);

        }

        public ChangeGroupChatImageCommandHandler(IAppDbContext context) : base(context)
        {
        }
    }
}
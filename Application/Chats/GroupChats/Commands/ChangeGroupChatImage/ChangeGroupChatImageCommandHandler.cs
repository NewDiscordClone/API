using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.RegularExpressions;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.GroupChats.Commands.ChangeGroupChatImage
{
    public class ChangeGroupChatImageCommandHandler : RequestHandlerBase, IRequestHandler<ChangeGroupChatImageCommand, Chat>
    {
        public async Task<Chat> Handle(ChangeGroupChatImageCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(command.ChatId, cancellationToken);

            string? oldImage = chat.Image;
            chat.Image = command.NewImage;

            await Context.GroupChats.UpdateAsync(chat, cancellationToken);

            if (oldImage != null)
            {
                string id = Regexes.ObjectIdRegex.Match(oldImage).Value;
                if (!Constants.Chats.DefaultImages.Contains(id))
                    await Context.CheckRemoveMedia(oldImage[(oldImage.LastIndexOf('/') - 1)..]);
            }

            return chat;
        }

        public ChangeGroupChatImageCommandHandler(IAppDbContext context) : base(context)
        {
        }
    }
}
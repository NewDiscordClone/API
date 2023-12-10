using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Chats.GroupChats.Commands.RenameGroupChat
{
    public class RenameGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<RenameGroupChatCommand, Chat>
    {
        public async Task<Chat> Handle(RenameGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(command.ChatId, cancellationToken);

            chat.Title = command.NewTitle;

            await Context.GroupChats.UpdateAsync(chat, cancellationToken);

            return chat;
        }

        public RenameGroupChatCommandHandler(IAppDbContext context) : base(
            context)
        {
        }
    }
}

using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.GroupChats.Commands.RenameGroupChat
{
    public class RenameGroupChatCommandHandler(IChatRepository chatRepository) : IRequestHandler<RenameGroupChatCommand, GroupChat>
    {
        private readonly IChatRepository _chatRepository = chatRepository;
        public async Task<GroupChat> Handle(RenameGroupChatCommand command, CancellationToken cancellationToken)
        {
            GroupChat chat = await _chatRepository.FindAsync<GroupChat>(command.ChatId, cancellationToken);

            chat.Title = command.NewTitle;

            await _chatRepository.UpdateAsync(chat, cancellationToken);

            return chat;
        }
    }
}

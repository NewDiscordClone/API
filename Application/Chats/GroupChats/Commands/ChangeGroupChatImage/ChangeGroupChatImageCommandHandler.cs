using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.GroupChats.Commands.ChangeGroupChatImage
{
    public class ChangeGroupChatImageCommandHandler(IChatRepository chatRepository)
        : IRequestHandler<ChangeGroupChatImageCommand, GroupChat>
    {
        private readonly IChatRepository _chatRepository = chatRepository;
        public async Task<GroupChat> Handle(ChangeGroupChatImageCommand command, CancellationToken cancellationToken)
        {
            GroupChat chat = await _chatRepository.FindAsync<GroupChat>(command.ChatId, cancellationToken);

            if (chat is not GroupChat groupChat)
                throw new InvalidOperationException();

            string? oldImage = groupChat.Image;
            groupChat.Image = command.NewImage;

            await _chatRepository.UpdateAsync(groupChat, cancellationToken);

            if (oldImage != null)
                await Context.CheckRemoveMedia(oldImage[(oldImage.LastIndexOf('/') - 1)..]);

            return groupChat;
        }
    }
}
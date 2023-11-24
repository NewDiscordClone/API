using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.GroupChats.Commands.ChangeGroupChatImage
{
    public class ChangeGroupChatImageCommandHandler(IChatRepository chatRepository)
        : IRequestHandler<ChangeGroupChatImageCommand, (GroupChat, string?)>
    {
        private readonly IChatRepository _chatRepository = chatRepository;
        public async Task<(GroupChat, string?)> Handle(ChangeGroupChatImageCommand command, CancellationToken cancellationToken)
        {
            GroupChat chat = await _chatRepository.FindAsync<GroupChat>(command.ChatId, cancellationToken);

            if (chat is not GroupChat groupChat)
                throw new InvalidOperationException();

            string? oldImage = groupChat.Image;
            groupChat.Image = command.NewImage;

            await _chatRepository.UpdateAsync(groupChat, cancellationToken);
            //TODO Remove old image
            return (groupChat, oldImage);
        }
    }
}
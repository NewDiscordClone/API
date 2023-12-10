using Sparkle.Domain;

namespace Sparkle.Application.Chats.GroupChats.Commands.RemoveUserFromGroupChat
{
    public record RemoveUserFromGroupChatCommandResult(
        GroupChat Chat,
        Guid UserId);
}

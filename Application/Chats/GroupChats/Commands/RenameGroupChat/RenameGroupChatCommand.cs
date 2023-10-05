using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.Chats.GroupChats.Commands.RenameGroupChat
{
    public class RenameGroupChatCommand : IRequest
    {
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        [DefaultValue("NewTitle")]
        public string? NewTitle { get; init; }
    }
}
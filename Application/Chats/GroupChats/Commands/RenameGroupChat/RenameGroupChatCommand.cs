using MediatR;
using Sparkle.Application.Models;
using System.ComponentModel;

namespace Sparkle.Application.Chats.GroupChats.Commands.RenameGroupChat
{
    public class RenameGroupChatCommand : IRequest<Chat>
    {
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        [DefaultValue("NewTitle")]
        public string? NewTitle { get; init; }
    }
}
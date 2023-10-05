using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.Chats.GroupChats.Commands.AddMemberToGroupChat
{
    public record AddMemberToGroupChatCommand : IRequest
    {
        /// <summary>
        /// The unique identifier of the group chat to add a new member to
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// The unique identifier of the new member to be added
        /// </summary>
        public Guid NewMemberId { get; init; }
    }
}

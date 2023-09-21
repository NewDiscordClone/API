using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.GroupChats.Commands.RemoveGroupChatMember
{
    public record RemoveGroupChatMemberCommand : IRequest
    {
        /// <summary>
        /// The unique identifier of the group chat to remove a member from.
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// The unique identifier of the member to be removed from the group chat.
        /// </summary>
        public Guid MemberId { get; init; }

        /// <summary>
        /// Indicates whether to remove the member silently without sending notifications. (Optional, default is false)
        /// </summary>
        [DefaultValue(false)]
        public bool Silent { get; init; } = false;
    }
}

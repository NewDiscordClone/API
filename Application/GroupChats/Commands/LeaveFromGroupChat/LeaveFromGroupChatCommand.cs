using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.GroupChats.Commands.LeaveFromGroupChat
{
    public record LeaveFromGroupChatCommand : IRequest
    {
        /// <summary>
        /// The unique identifier of the group chat to leave from.
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        public Guid ProfileId { get; init; }

        /// <summary>
        /// Indicates whether to leave the group chat silently without sending notifications. (Optional, default is false)
        /// </summary>
        [DefaultValue(false)]
        public bool Silent { get; init; } = false;
    }
}

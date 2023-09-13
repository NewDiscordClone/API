using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.GroupChats.Commands.LeaveFromGroupChat
{
    public record LeaveFromGroupChatRequest : IRequest
    {
        /// <summary>
        /// The unique identifier of the group chat to leave from.
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// Indicates whether to leave the group chat silently without sending notifications. (Optional, default is false)
        /// </summary>
        [DefaultValue(false)]
        public bool Silent { get; init; } = false;
    }
}

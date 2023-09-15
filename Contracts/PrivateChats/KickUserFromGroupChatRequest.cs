using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Contracts.PrivateChats
{
    public record KickUserFromGroupChatRequest
    {
        /// <summary>
        /// The unique identifier of the member to be removed from the group chat.
        /// </summary>
        [Required]
        public Guid MemberId { get; init; }

        /// <summary>
        /// Indicates whether to remove the member silently without sending notifications. (Optional, default is false)
        /// </summary>
        [DefaultValue(false)]
        public bool Silent { get; init; } = false;
    }
}

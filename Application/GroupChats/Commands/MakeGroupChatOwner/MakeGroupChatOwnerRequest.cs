using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.GroupChats.Commands.MakeGroupChatOwner
{
    public record MakeGroupChatOwnerRequest : IRequest
    {
        /// <summary>
        /// The unique identifier of the group chat to change an owner in.
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// The unique identifier of the member to be made the owner.
        /// </summary>
        [Required]
        public Guid MemberId { get; init; }
    }
}

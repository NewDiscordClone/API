using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.GroupChats.Commands.AddMemberToGroupChat
{
    public record AddMemberToGroupChatRequest : IRequest
    {
        /// <summary>
        /// The unique identifier of the group chat to add a new member to
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// The unique identifier of the new member to be added
        /// </summary>
        [Required]
        public Guid NewMemberId { get; init; }
    }
}

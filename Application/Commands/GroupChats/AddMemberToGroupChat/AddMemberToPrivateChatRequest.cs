using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.GroupChats.AddMemberToGroupChat
{
    public record AddMemberToGroupChatRequest : IRequest
    {
        /// <summary>
        /// The unique identifier of the group chat to which to add a new member
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// The unique identifier of the new member to be added
        /// </summary>
        [Required]
        [DefaultValue(1)]
        public int NewMemberId { get; init; }
    }
}

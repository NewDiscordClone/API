﻿using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.GroupChats.Commands.RemoveGroupChatMember
{
    public record RemoveGroupChatMemberRequest : IRequest
    {
        /// <summary>
        /// The unique identifier of the group chat to remove a member from.
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

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

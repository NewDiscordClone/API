﻿using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.GroupChats.ChangeGroupChatImage
{
    public record ChangeGroupChatImageRequest : IRequest
    {
        /// <summary>
        /// The unique identifier of the group chat for which to change the image
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// The URL of the new image for the group chat
        /// </summary>
        [Required]
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string NewImage { get; init; }
    }
}

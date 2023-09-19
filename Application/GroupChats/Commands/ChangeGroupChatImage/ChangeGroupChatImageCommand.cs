using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.GroupChats.Commands.ChangeGroupChatImage
{
    public record ChangeGroupChatImageCommand : IRequest
    {
        /// <summary>
        /// The unique identifier of the group chat to change the image for
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// The URL of the new image for the group chat
        /// </summary>
        [DataType(DataType.ImageUrl)]
        public string NewImage { get; init; }
    }
}

using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Channels.RenameChannel
{
    public record RenameChannelRequest : IRequest
    {
        /// <summary>
        /// Id of the channel to be renamed
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// New name of the channel
        /// </summary>
        [Required]
        [MaxLength(100)]
        [DefaultValue("NewTitle")]
        public string NewTitle { get; init; }
    }
}
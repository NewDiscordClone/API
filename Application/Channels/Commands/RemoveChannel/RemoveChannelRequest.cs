using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Channels.Commands.RemoveChannel
{
    public class RemoveChannelRequest : IRequest
    {
        /// <summary>
        /// Id of the channel to be removed 
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
    }
}
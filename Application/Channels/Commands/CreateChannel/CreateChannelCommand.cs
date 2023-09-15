using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Channels.Commands.CreateChannel
{
    public class CreateChannelCommand : IRequest<string>
    {
        /// <summary>
        /// Name of the channel 
        /// </summary>
        [Required]
        [MaxLength(100)]
        [DefaultValue("Channel")]
        public string Title { get; init; }

        /// <summary>
        /// Id of the server where chat will be created
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
    }
}
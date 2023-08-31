using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Models;
using MediatR;

namespace Application.Commands.Channels.CreateChannel
{
    public class CreateChannelRequest : IRequest<Channel>
    {
        [Required]
        [MaxLength(100)] 
        [DefaultValue("Channel")]
        public string Title { get; init; }

        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
    }
}
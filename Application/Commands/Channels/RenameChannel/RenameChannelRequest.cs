using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.Channels.RenameChannel
{
    public class RenameChannelRequest : IRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
        
        [Required]
        [MaxLength(100)]
        [DefaultValue("NewTitle")]
        public string NewTitle { get; init; }
    }
}
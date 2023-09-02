using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.Servers.KickUser
{
    public class KickUserRequest : IRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; set; }
        [Required]
        [DefaultValue(1)]
        public int UserId { get; set; }
    }
}
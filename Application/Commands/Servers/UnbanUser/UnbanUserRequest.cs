using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.Servers.UnbanUser
{
    public record UnbanUserRequest : IRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}
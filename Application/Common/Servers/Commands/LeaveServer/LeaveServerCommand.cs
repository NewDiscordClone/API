using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Common.Servers.Commands.LeaveServer
{
    public record LeaveServerCommand : IRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
    }
}
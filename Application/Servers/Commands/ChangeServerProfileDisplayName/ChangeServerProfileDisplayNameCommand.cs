using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.Servers.Commands.ChangeServerProfileDisplayName
{
    public record ChangeServerProfileDisplayNameCommand : IRequest
    {
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
        public string NewDisplayName { get; init; }
        public Guid? UserId { get; set; }
    }
}
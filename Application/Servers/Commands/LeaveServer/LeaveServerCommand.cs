using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.Servers.Commands.LeaveServer
{
    public record LeaveServerCommand : IRequest
    {
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
    }
}
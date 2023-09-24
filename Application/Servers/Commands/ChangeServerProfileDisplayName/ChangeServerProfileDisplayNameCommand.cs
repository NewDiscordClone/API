using MediatR;

namespace Sparkle.Application.Servers.Commands.ChangeServerProfileDisplayName
{
    public record ChangeServerProfileDisplayNameCommand : IRequest
    {
        public Guid ProfileId { get; init; }
        public string NewDisplayName { get; init; }
    }
}
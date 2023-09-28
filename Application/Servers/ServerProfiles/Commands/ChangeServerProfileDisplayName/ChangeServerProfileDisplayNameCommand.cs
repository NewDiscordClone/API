using MediatR;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileDisplayName
{
    public record ChangeServerProfileDisplayNameCommand : IRequest
    {
        public Guid ProfileId { get; init; }
        public string? NewDisplayName { get; init; }
    }
}
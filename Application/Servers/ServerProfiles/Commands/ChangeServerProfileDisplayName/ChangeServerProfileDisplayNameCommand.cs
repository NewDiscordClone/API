using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileDisplayName
{
    public record ChangeServerProfileDisplayNameCommand : IRequest<ServerProfile>
    {
        public Guid ProfileId { get; init; }
        public string? NewDisplayName { get; init; }
    }
}
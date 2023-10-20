using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.BanUser
{
    public record BanUserCommand : IRequest<ServerProfile>
    {
        public string ServerId { get; init; }
        public Guid ProfileId { get; init; }
    }
}
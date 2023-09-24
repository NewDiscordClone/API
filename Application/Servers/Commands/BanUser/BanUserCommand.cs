using MediatR;

namespace Sparkle.Application.Servers.Commands.BanUser
{
    public record BanUserCommand : IRequest
    {
        public string ServerId { get; init; }
        public Guid ProfileId { get; init; }
    }
}
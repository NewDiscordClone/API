using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Commands
{
    public record ChangeStatusCommand : IRequest
    {
        public UserStatus Status { get; init; }
    }
}
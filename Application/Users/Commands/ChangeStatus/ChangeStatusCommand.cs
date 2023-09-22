using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands.ChangeStatus
{
    public record ChangeStatusCommand : IRequest
    {
        public UserStatus Status { get; init; }
    }
}
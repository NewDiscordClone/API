using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands.ChangeDisplayName
{
    public record ChangeDisplayNameCommand : IRequest<User>
    {
        public string? DisplayName { get; set; }
    }
}

using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Commands
{
    public record ChangeDisplayNameCommand : IRequest<User>
    {
        public string? DisplayName { get; set; }
    }
}

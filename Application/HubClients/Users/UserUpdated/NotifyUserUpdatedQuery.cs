using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Users.UserUpdated
{
    public record NotifyUserUpdatedQuery : IRequest
    {
        public User UpdatedUser { get; init; }
    }
}
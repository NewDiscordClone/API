using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.Users.UserUpdated
{
    public record NotifyUserUpdatedQuery : IRequest
    {
        public User UpdatedUser { get; init; }
    }
}
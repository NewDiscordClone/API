using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.Servers.ServerDeleted
{
    public record NotifyServerDeletedEvent(Server Server, IEnumerable<Guid> UserIds) : INotification
    {
    }
}
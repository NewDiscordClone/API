using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Servers.ServerDeleted
{
    public record NotifyServerDeletedEvent(Server Server, IEnumerable<Guid> UserIds) : INotification
    {
    }
}
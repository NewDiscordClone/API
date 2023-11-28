using MediatR;

namespace Sparkle.Application.Models.Events
{
    public record ServerDeletedEvent(Server Server, IEnumerable<Guid> UserIds) : INotification;
    public record ServerUpdatedEvent(Server Server) : INotification;
    public record ServerCreatedEvent(Server Server) : INotification;
}

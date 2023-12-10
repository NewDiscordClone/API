using MediatR;

namespace Sparkle.Domain.Events
{
    public record ProfileUpdatedEvent(ServerProfile Profile) : INotification;
    public record ProfileDeletedEvent(ServerProfile Profile) : INotification;
}

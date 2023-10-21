using MediatR;

namespace Sparkle.Application.Models.Events
{
    public record ProfileUpdatedEvent(ServerProfile Profile) : INotification;
    public record ProfileDeletedEvent(ServerProfile Profile) : INotification;
}

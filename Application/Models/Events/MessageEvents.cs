using MediatR;

namespace Sparkle.Application.Models.Events
{
    public record MessageSentEvent(Message Message) : INotification;
    public record MessageUpdatedEvent(Message Message) : INotification;
    public record MessageRemovedEvent(Message Message) : INotification;
}

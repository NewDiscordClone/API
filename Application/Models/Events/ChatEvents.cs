using MediatR;

namespace Sparkle.Application.Models.Events
{
    public record PersonalChatCreated(PersonalChat Chat) : INotification;
    public record PersonalChatUpdated(PersonalChat Chat) : INotification;
    public record PersonalChatUserRemoved(PersonalChat Chat, Guid RemovedUserId) : INotification;
}

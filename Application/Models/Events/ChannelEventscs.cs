using MediatR;

namespace Sparkle.Application.Models.Events
{
    public record ChannelCreatedEvent(Channel Channel) : INotification;
    public record ChannelUpdatedEvent(Channel Channel) : INotification;
    public record ChannelRemovedEvent(Channel Channel) : INotification;
}

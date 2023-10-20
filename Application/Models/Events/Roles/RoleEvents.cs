using AutoMapper;
using MediatR;

namespace Sparkle.Application.Models.Events.Roles
{
    public record RoleCreatedEvent(Role Role) : INotification;
    public record RoleUpdatedEvent(Role Role) : INotification;
    public record RoleDeletedEvent(Role Role) : INotification;
    public record RoleAssignedEvent(Role Role, Profile Profile) : INotification;
    public record RoleUnassignedEvent(Role Role, Profile Profile) : INotification;

}

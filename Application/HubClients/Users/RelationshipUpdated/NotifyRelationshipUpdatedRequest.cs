using MediatR;

namespace Sparkle.Application.HubClients.Users.RelationshipUpdated
{
    public record NotifyRelationshipUpdatedRequest : IRequest
    {
        public Guid UserId { get; init; }
    }
}
using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.Users.RelationshipUpdated
{
    public record NotifyRelationshipUpdatedQuery : IRequest
    {
        public Relationship Relationship { get; init; }
    }
}
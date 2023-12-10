using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.Users.RelationshipDeleted
{
    public record NotifyRelationshipDelatedQuery : IRequest
    {
        public Relationship Relationship { get; init; }
    }
}

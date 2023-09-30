using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Users.RelationshipUpdated
{
    public record NotifyRelationshipUpdatedQuery : IRequest
    {
        public Relationship Relationship { get; init; }
    }
}
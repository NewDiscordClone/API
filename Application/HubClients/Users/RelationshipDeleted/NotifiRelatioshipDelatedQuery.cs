using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Users.RelationshipDeleted
{
    public record NotifyRelationshipDelatedQuery : IRequest
    {
        public Relationship Relationship { get; init; }
    }
}

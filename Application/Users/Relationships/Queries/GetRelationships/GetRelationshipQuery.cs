using MediatR;
using Sparkle.Contracts.Users.Relationships;

namespace Sparkle.Application.Users.Relationships.Queries.GetRelationships
{
    public record GetRelationshipQuery()
        : IRequest<List<RelationshipViewModel>>;
}
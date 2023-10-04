using MediatR;

namespace Sparkle.Application.Users.Relationships.Queries.GetRelationships
{
    public record GetRelationshipQuery()
        : IRequest<List<RelationshipViewModel>>;
}
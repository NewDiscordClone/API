using MediatR;

namespace Sparkle.Application.Users.Relationships.Queries
{
    public record GetRelationshipQuery()
        : IRequest<List<RelationshipViewModel>>;
}
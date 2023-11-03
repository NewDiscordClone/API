using MediatR;

namespace Sparkle.Application.Users.Relationships.Queries
{
    public record GetRelationshipQuery(bool IncludeAcquaintance)
        : IRequest<List<RelationshipViewModel>>;
}
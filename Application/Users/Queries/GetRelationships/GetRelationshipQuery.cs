using MediatR;

namespace Sparkle.Application.Users.Queries.GetRelationships
{
    public record GetRelationshipQuery()
        : IRequest<List<RelationshipDto>>;
}
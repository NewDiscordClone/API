using MediatR;

namespace Sparkle.Application.Users.Queries.GetRelationships
{
    public record GetRelationshipRequest()
        : IRequest<List<RelationshipDto>>;
}
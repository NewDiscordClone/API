using MediatR;

namespace Application.Queries.GetRelationships
{
    public record GetRelationshipRequest()
        : IRequest<List<RelationshipDto>>;
}
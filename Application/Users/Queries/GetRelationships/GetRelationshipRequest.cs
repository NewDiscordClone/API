using MediatR;

namespace Application.Users.Queries.GetRelationships
{
    public record GetRelationshipRequest()
        : IRequest<List<RelationshipDto>>;
}
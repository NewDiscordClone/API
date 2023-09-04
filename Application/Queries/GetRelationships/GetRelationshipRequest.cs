using MediatR;

namespace Application.Queries.GetRelationships
{
    public class GetRelationshipRequest : IRequest<List<RelationshipDto>>
    {
        
    }
}
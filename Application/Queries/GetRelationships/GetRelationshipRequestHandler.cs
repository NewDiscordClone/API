using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetRelationships
{
    public class GetRelationshipRequestHandler : RequestHandlerBase,
        IRequestHandler<GetRelationshipRequest, List<RelationshipDto>>
    {
        public GetRelationshipRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(context, userProvider, mapper)
        {
        }

        public async Task<List<RelationshipDto>> Handle(GetRelationshipRequest request,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            RelationshipList relationships = await Context.RelationshipLists.FindAsync(UserId);
            List<RelationshipDto> relationshipDtos = new List<RelationshipDto>();
            foreach (var relationship in relationships.Relationships)
            {
                relationshipDtos.Add(new RelationshipDto()
                {
                    User = Mapper.Map<UserLookUp>(
                        Context.FindSqlByIdAsync<User>(relationship.UserId, cancellationToken)),
                    RelationshipType = relationship.RelationshipType
                });
            }

            return relationshipDtos;
        }
    }
}
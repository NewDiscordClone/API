using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Contracts.Users.Relationships;

namespace Sparkle.Application.Users.Relationships.Queries.GetRelationships
{
    public class GetRelationshipQueryHandler : RequestHandlerBase,
        IRequestHandler<GetRelationshipQuery, List<RelationshipViewModel>>
    {
        public GetRelationshipQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider)
            : base(context, userProvider)
        {
        }

        public async Task<List<RelationshipViewModel>> Handle(GetRelationshipQuery query, CancellationToken cancellationToken)
        {
            List<Relationship> relationships = await Context.Relationships
                .Where(rel => rel.Active == UserId || rel.Passive == UserId)
                .ToListAsync(cancellationToken);

            return relationships.ConvertAll(ToViewModel);
        }

        private RelationshipViewModel ToViewModel(Relationship relationship)
        {
            User? user = Context.Users
                .Find(relationship.Active == UserId
                ? relationship.Passive : relationship.Active);

            return new RelationshipViewModel
            {
                IsActive = relationship.Active != UserId,
                User = Mapper.Map<UserLookupViewModel>(user),
                Type = relationship.RelationshipType
            };
        }
    }
}
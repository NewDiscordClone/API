using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Queries.GetRelationships
{
    public class GetRelationshipQueryHandler : RequestHandlerBase,
        IRequestHandler<GetRelationshipQuery, List<Relationship>>
    {
        public GetRelationshipQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider)
            : base(context, userProvider)
        {
        }

        public async Task<List<Relationship>> Handle(GetRelationshipQuery query, CancellationToken cancellationToken)
        {
            return await Context.Relationships
                .Where(rel => rel.Active == UserId || rel.Passive == UserId)
                .ToListAsync(cancellationToken);
        }
    }
}
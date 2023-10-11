using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Queries.GetRelationships
{
    public class GetRelationshipQueryHandler : RequestHandlerBase,
        IRequestHandler<GetRelationshipQuery, List<RelationshipViewModel>>
    {
        private readonly IConvertor _convertor;

        public GetRelationshipQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IConvertor convertor)
            : base(context, userProvider)
        {
            _convertor = convertor;
        }

        public async Task<List<RelationshipViewModel>> Handle(GetRelationshipQuery query, CancellationToken cancellationToken)
        {
            List<Relationship> relationships = await Context.Relationships
                .Where(rel => rel.Active == UserId || rel.Passive == UserId)
                .ToListAsync(cancellationToken);

            return relationships.ConvertAll(_convertor.Convert);
        }
    }
}
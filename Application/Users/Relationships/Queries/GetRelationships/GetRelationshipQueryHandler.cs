using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Relationships.Queries
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
            IQueryable<Relationship> relationshipsQuery = Context.Relationships
                .Where(relationship => relationship.Active == UserId || relationship.Passive == UserId);

            if (!query.IncludeAcquaintance)
            {
                relationshipsQuery = relationshipsQuery.Where(relationship
                    => relationship.RelationshipType != RelationshipTypes.Acquaintance);
            }

            List<Relationship> relationships = await relationshipsQuery.ToListAsync(cancellationToken);

            return relationships.ConvertAll(relationship => _convertor.Convert(relationship));
        }
    }
}
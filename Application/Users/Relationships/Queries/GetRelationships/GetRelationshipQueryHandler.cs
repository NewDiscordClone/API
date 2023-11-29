using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Queries
{
    public class GetRelationshipQueryHandler : RequestHandler,
        IRequestHandler<GetRelationshipQuery, List<RelationshipViewModel>>
    {
        private readonly IConvertor _convertor;
        private readonly IRelationshipRepository _relationshipRepository;

        public GetRelationshipQueryHandler(IAuthorizedUserProvider userProvider,
            IConvertor convertor,
            IRelationshipRepository relationshipRepository)
            : base(userProvider)
        {
            _convertor = convertor;
            _relationshipRepository = relationshipRepository;
        }

        public async Task<List<RelationshipViewModel>> Handle(GetRelationshipQuery query, CancellationToken cancellationToken)
        {
            IQueryable<Relationship> relationshipsQuery = _relationshipRepository.ExecuteCustomQuery(relationships => relationships
                .Where(relationship => relationship.Active == UserId || relationship.Passive == UserId));

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
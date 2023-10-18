using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Users.Relationships.Queries.GetRelationships;

namespace Sparkle.Application.HubClients.Users.RelationshipDeleted
{
    public class NotifyRelationshipDelatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyRelationshipDelatedQuery>
    {
        private readonly IConvertor _convertor;
        public NotifyRelationshipDelatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context,
            IConvertor convertor, IAuthorizedUserProvider userProvider)
            : base(hubContextProvider, context, userProvider)
        {
            _convertor = convertor;
        }

        public async Task Handle(NotifyRelationshipDelatedQuery query, CancellationToken cancellationToken)
        {
            Relationship relationship = query.Relationship;

            Guid notCurrentUserId = relationship.Active == UserId ?
               relationship.Passive : relationship.Active;

            RelationshipViewModel modelForCurrentUser = _convertor.Convert(relationship);
            RelationshipViewModel modelForOtherUser = _convertor.Convert(relationship, notCurrentUserId);

            await SendAsync(ClientMethods.RelationshipsDeleted, modelForCurrentUser, GetConnections(UserId));
            await SendAsync(ClientMethods.RelationshipsDeleted, modelForOtherUser, GetConnections(notCurrentUserId));
        }
    }
}

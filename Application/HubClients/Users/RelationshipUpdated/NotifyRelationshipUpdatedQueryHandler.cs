using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;
using Sparkle.Application.Users.Relationships.Queries;

namespace Sparkle.Application.HubClients.Users.RelationshipUpdated
{
    public class NotifyRelationshipUpdatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyRelationshipUpdatedQuery>
    {
        private readonly IConvertor _convertor;
        public NotifyRelationshipUpdatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context,
            IConvertor convertor, IAuthorizedUserProvider userProvider)
            : base(hubContextProvider, context, userProvider)
        {
            _convertor = convertor;
        }

        public async Task Handle(NotifyRelationshipUpdatedQuery query, CancellationToken cancellationToken)
        {
            Relationship relationship = query.Relationship;

            Guid notCurrentUserId = relationship.Active == UserId ?
                relationship.Passive : relationship.Active;

            RelationshipViewModel modelForCurrentUser = _convertor.Convert(relationship);
            RelationshipViewModel modelForOtherUser = _convertor.Convert(relationship, notCurrentUserId);

            await SendAsync(ClientMethods.RelationshipsUpdated, modelForCurrentUser, GetConnections(UserId));
            await SendAsync(ClientMethods.RelationshipsUpdated, modelForOtherUser, GetConnections(notCurrentUserId));
        }
    }
}
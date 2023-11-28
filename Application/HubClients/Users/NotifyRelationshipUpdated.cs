using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;
using Sparkle.Application.Users.Relationships.Queries;

namespace Sparkle.Application.HubClients.Users
{
    public class NotifyRelationshipUpdated : HubHandler, INotificationHandler<RelationshipUpdatedEvent>
    {
        private readonly IConvertor _convertor;
        public NotifyRelationshipUpdated(IHubContextProvider hubContextProvider,
            IConnectionsRepository connectionsRepository,
            IConvertor convertor,
            IAuthorizedUserProvider userProvider)
            : base(hubContextProvider, userProvider, connectionsRepository)
        {
            _convertor = convertor;
        }

        public async Task Handle(RelationshipUpdatedEvent notification, CancellationToken cancellationToken)
        {
            Relationship relationship = notification.Relationship;

            Guid notCurrentUserId = relationship.Active == UserId ?
                relationship.Passive : relationship.Active;

            RelationshipViewModel modelForCurrentUser = _convertor.Convert(relationship);
            RelationshipViewModel modelForOtherUser = _convertor.Convert(relationship, notCurrentUserId);

            IEnumerable<string> currentUserConnections = await ConnectionsRepository
                .FindConnectionsAsync(UserId, cancellationToken);
            IEnumerable<string> otherUserConnections = await ConnectionsRepository
                .FindConnectionsAsync(notCurrentUserId, cancellationToken);

            await SendAsync(ClientMethods.RelationshipsUpdated, modelForCurrentUser, currentUserConnections, cancellationToken);
            await SendAsync(ClientMethods.RelationshipsUpdated, modelForOtherUser, otherUserConnections, cancellationToken);
        }
    }
}
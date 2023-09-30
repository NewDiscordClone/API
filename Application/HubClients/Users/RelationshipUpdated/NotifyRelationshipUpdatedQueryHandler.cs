using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Users.RelationshipUpdated
{
    public class NotifyRelationshipUpdatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyRelationshipUpdatedQuery>
    {
        public NotifyRelationshipUpdatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context)
            : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyRelationshipUpdatedQuery query, CancellationToken cancellationToken)
        {
            Relationship relationship = query.Relationship;

            await SendAsync(ClientMethods.RelationshipsUpdated, relationship,
                GetConnections(relationship.Active, relationship.Passive));
        }
    }
}
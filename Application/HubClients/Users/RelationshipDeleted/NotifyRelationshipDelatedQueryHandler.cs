using MediatR;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application.HubClients.Users.RelationshipDeleted
{
    public class NotifyRelationshipDelatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyRelationshipDelatedQuery>
    {
        public NotifyRelationshipDelatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context)
            : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyRelationshipDelatedQuery query, CancellationToken cancellationToken)
        {
            Models.Relationship relationship = query.Relationship;

            await SendAsync(ClientMethods.RelationshipsDeleted, relationship,
                GetConnections(relationship.UserPassive, relationship.UserPassive));
        }
    }
}

using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Users.Relationships.Queries.GetRelationships;

namespace Sparkle.Application.HubClients.Users.RelationshipUpdated
{
    public class NotifyRelationshipUpdatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyRelationshipUpdatedQuery>
    {
        private readonly IRelationshipConvertor _convertor;
        public NotifyRelationshipUpdatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context,
            IRelationshipConvertor convertor)
            : base(hubContextProvider, context)
        {
            _convertor = convertor;
        }

        public async Task Handle(NotifyRelationshipUpdatedQuery query, CancellationToken cancellationToken)
        {
            Relationship relationship = query.Relationship;
            RelationshipViewModel viewModel = _convertor.Convert(relationship);

            await SendAsync(ClientMethods.RelationshipsUpdated, viewModel,
                GetConnections(relationship.Active, relationship.Passive));
        }
    }
}
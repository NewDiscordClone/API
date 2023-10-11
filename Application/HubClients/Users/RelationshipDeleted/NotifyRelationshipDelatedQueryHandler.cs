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
            IConvertor convertor)
            : base(hubContextProvider, context)
        {
            _convertor = convertor;
        }

        public async Task Handle(NotifyRelationshipDelatedQuery query, CancellationToken cancellationToken)
        {
            Relationship relationship = query.Relationship;
            RelationshipViewModel viewModel = _convertor.Convert(relationship);

            await SendAsync(ClientMethods.RelationshipsDeleted, viewModel,
                GetConnections(relationship.Passive, relationship.Passive));
        }
    }
}

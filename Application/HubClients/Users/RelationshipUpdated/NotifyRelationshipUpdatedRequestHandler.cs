using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;
using Sparkle.Application.Users.Queries.GetRelationships;

namespace Sparkle.Application.HubClients.Users.RelationshipUpdated
{
    public class NotifyRelationshipUpdatedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyRelationshipUpdatedRequest>
    {
        public NotifyRelationshipUpdatedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(hubContextProvider, context, userProvider, mapper)
        {
        }

        public async Task Handle(NotifyRelationshipUpdatedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            RelationshipList? relationships =
                await Context.RelationshipLists.FindOrDefaultAsync(request.UserId) ??
                new RelationshipList()
                {
                    Id = request.UserId,
                    Relationships = new List<Relationship>()
                };
            List<RelationshipDto> relationshipDtos = new();
            foreach (Relationship relationship in relationships.Relationships)
            {
                relationshipDtos.Add(new RelationshipDto()
                {
                    User = Mapper.Map<UserLookUp>(
                        await Context.SqlUsers.FindAsync(relationship.UserId)),
                    RelationshipType = relationship.RelationshipType
                });
            }

            await SendAsync(ClientMethods.RelationshipsUpdated, relationshipDtos, GetConnections(request.UserId));
        }
    }
}
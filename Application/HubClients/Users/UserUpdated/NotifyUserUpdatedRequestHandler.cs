using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Users.UserUpdated
{
    public class NotifyUserUpdatedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyUserUpdatedRequest>
    {
        public NotifyUserUpdatedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(hubContextProvider, context, userProvider, mapper)
        {
        }

        public async Task Handle(NotifyUserUpdatedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            RelationshipList relationshipsList = await Context.RelationshipLists.FindAsync(UserId);
            User user = await Context.SqlUsers.FindAsync(UserId);
            var notifyArg = Mapper.Map<UserLookUp>(user);
            await SendAsync(ClientMethods.UserUpdated, notifyArg,
                GetConnections(UserId));
            foreach (var relationship in relationshipsList.Relationships)
            {
                await SendAsync(ClientMethods.UserUpdated, notifyArg,
                    GetConnections(relationship.UserId));
            }
        }
    }
}
using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.ChangeStatus
{
    public class NotifyChangeStatusRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyChangeStatusRequest>
    {
        public NotifyChangeStatusRequestHandler(
            IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IAuthorizedUserProvider userProvider) :
            base(hubContextProvider, context, userProvider)
        {
        }

        public async Task Handle(NotifyChangeStatusRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            User user = await Context.SqlUsers.FindAsync(UserId);
            RelationshipList relationshipsList = await Context.RelationshipLists.FindAsync(UserId);
            user.Status = request.Status;
            await Context.SqlUsers.UpdateAsync(user);
            var notifyArg = new { UserId, request.Status };
            foreach (var relationship in relationshipsList.Relationships)
            {
                await SendAsync(ClientMethods.StatusChanged, notifyArg,
                    GetConnections(relationship.UserId));
            }
        }
    }
}
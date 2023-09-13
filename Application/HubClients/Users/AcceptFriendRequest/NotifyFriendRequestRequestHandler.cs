using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Users.AcceptFriendRequest
{
    public class NotifyAcceptFriendRequestRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyAcceptFriendRequestRequest>
    {
        public NotifyAcceptFriendRequestRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(hubContextProvider, context, userProvider, mapper)
        {
        }

        public async Task Handle(NotifyAcceptFriendRequestRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            User user = await Context.SqlUsers.FindAsync(UserId);

            await SendAsync(ClientMethods.AcceptFriendRequest, Mapper.Map<UserLookUp>(user), GetConnections(request.UserId));
        }
    }
}
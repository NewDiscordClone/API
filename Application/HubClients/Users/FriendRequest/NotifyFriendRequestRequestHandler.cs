using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Users.FriendRequest
{
    public class NotifyFriendRequestRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyFriendRequestRequest>
    {
        public NotifyFriendRequestRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(hubContextProvider, context, userProvider, mapper)
        {
        }

        public async Task Handle(NotifyFriendRequestRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            User user = await Context.SqlUsers.FindAsync(UserId);

            await SendAsync(ClientMethods.FriendRequest, Mapper.Map<UserViewModel>(user), GetConnections(request.UserId));
        }
    }
}
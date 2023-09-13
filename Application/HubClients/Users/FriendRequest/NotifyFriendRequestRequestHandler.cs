using Application.Application;
using Application.Common.Interfaces;
using Application.HubClients;
using Application.Models;
using Application.Models.LookUps;
using AutoMapper;
using MediatR;

namespace Application.HubClients.Users.FriendRequest
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
            User other = await Context.SqlUsers.FindAsync(request.UserId);

            await SendAsync(ClientMethods.FriendRequest, Mapper.Map<UserLookUp>(user), GetConnections(other));
        }
    }
}
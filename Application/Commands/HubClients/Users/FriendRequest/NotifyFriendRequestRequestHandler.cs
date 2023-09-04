using Application.Application;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.HubClients.Users.FriendRequest
{
    public class NotifyFriendRequestRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyFriendRequestRequest>
    {
        public NotifyFriendRequestRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(hubContextProvider, context, userProvider, mapper)
        {
        }

        public async Task Handle(NotifyFriendRequestRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            User user = await Context.FindSqlByIdAsync<User>(UserId, cancellationToken);
            User other = await Context.FindSqlByIdAsync<User>(request.UserId, cancellationToken);

            await SendAsync(ClientMethods.FriendRequest, Mapper.Map<UserLookUp>(user), GetConnections(other));
        }
    }
}
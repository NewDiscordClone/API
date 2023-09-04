using Application.Application;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.HubClients.Users.AcceptFriendRequest
{
    public class NotifyAcceptFriendRequestRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyAcceptFriendRequestRequest>
    {
        public NotifyAcceptFriendRequestRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(hubContextProvider, context, userProvider, mapper)
        {
        }

        public async Task Handle(NotifyAcceptFriendRequestRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            User user = await Context.FindSqlByIdAsync<User>(UserId, cancellationToken);
            User other = await Context.FindSqlByIdAsync<User>(request.UserId, cancellationToken);

            await SendAsync(ClientMethods.AcceptFriendRequest, Mapper.Map<UserLookUp>(user), GetConnections(other));
        }
    }
}
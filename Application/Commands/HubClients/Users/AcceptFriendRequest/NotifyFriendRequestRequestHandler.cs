using Application.Application;
using Application.Interfaces;
using Application.Models;
using Application.Models.LookUps;
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
            User user = await Context.SqlUsers.FindAsync(UserId);
            User other = await Context.SqlUsers.FindAsync(request.UserId);

            await SendAsync(ClientMethods.AcceptFriendRequest, Mapper.Map<UserLookUp>(user), GetConnections(other));
        }
    }
}
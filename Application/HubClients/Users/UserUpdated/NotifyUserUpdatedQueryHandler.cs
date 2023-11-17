using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Users.UserUpdated
{
    public class NotifyUserUpdatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyUserUpdatedQuery>
    {
        public NotifyUserUpdatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context,
            IAuthorizedUserProvider userProvider, IMapper mapper) : base(hubContextProvider, context, userProvider,
            mapper)
        {
        }

        public async Task Handle(NotifyUserUpdatedQuery query, CancellationToken cancellationToken)
        {
            User user = query.UpdatedUser;
            UserViewModel notifyArg = Mapper.Map<UserViewModel>(user);

            await SendAsync(ClientMethods.UserUpdated, notifyArg, GetConnections(UserId));
        }
    }
}
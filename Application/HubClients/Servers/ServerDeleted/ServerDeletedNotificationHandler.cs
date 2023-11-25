using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Common;

namespace Sparkle.Application.HubClients.Servers.ServerDeleted
{
    public class ServerDeletedNotificationHandler : HubHandler, INotificationHandler<NotifyServerDeletedEvent>
    {
        public ServerDeletedNotificationHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyServerDeletedEvent query, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);

            await SendAsync(ClientMethods.ServerDeleted, query.Server.Id, GetConnections(query.UserIds));
        }
    }
}
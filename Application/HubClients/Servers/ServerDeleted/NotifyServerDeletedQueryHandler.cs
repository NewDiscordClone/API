using MediatR;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application.HubClients.Servers.ServerDeleted
{
    public class NotifyServerDeletedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyServerDeletedQuery>
    {
        public NotifyServerDeletedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyServerDeletedQuery query, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);

            await SendAsync(ClientMethods.ServerDeleted, query.Server.Id, GetConnections(query.Server));
        }
    }
}
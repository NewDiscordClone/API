using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.Servers.ServerUpdated
{
    public class NotifyServerUpdatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyServerUpdatedQuery>
    {
        public NotifyServerUpdatedQueryHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyServerUpdatedQuery query, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(query.ServerId);

            await SendAsync(ClientMethods.ServerUpdated, server, GetConnections(server));
        }
    }
}
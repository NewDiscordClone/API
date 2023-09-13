using Application.Application;
using Application.Common.Interfaces;
using Application.HubClients;
using Application.Models;
using MediatR;

namespace Application.HubClients.Servers.ServerUpdated
{
    public class NotifyServerUpdatedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyServerUpdatedRequest>
    {
        public NotifyServerUpdatedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyServerUpdatedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(request.ServerId);

            await SendAsync(ClientMethods.PrivateChatUpdated, server, GetConnections(server));
        }
    }
}
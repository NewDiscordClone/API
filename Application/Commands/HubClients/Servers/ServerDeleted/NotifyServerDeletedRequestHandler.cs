using Application.Application;
using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Commands.HubClients.Servers.ServerDeleted
{
    public class NotifyServerDeletedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyServerDeletedRequest>
    {
        public NotifyServerDeletedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyServerDeletedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);

            await SendAsync(ClientMethods.ServerDeleted, request.Server.Id, GetConnections(request.Server));
        }
    }
}
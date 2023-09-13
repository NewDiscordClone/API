using Application.Application;
using Application.Common.Interfaces;
using Application.HubClients;
using MediatR;

namespace Application.HubClients.Servers.ServerDeleted
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
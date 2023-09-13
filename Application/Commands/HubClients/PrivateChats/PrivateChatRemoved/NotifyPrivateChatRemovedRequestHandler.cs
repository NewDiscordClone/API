using Application.Application;
using Application.Interfaces;
using MediatR;

namespace Application.Commands.HubClients.PrivateChats.PrivateChatRemoved
{
    public class NotifyPrivateChatRemovedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyPrivateChatRemovedRequest>
    {
        public NotifyPrivateChatRemovedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context) : base(hubContextProvider, context)
        {
        }

        public async Task Handle(NotifyPrivateChatRemovedRequest request, CancellationToken cancellationToken)
        {
            await SendAsync(ClientMethods.PrivateChatRemoved, request.ChatId, GetConnections(request.UserId));
        }
    }
}
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Commands.NotifyClients.NotifyChatMembers
{
    public class NotifyMessageAddedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyMessageAddedRequest>
    {
        public async Task Handle(NotifyMessageAddedRequest request, CancellationToken cancellationToken)
        {
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken,
                "Chat",
                "Chat.Users"
            );
            IEnumerable<string> connectedUsers = GetConnections(message.Chat);

            foreach (string connectionId in connectedUsers)
            {
                await Clients.Client(connectionId).SendAsync("MessageAdded", Mapper.Map<NotifyMessageAddedDto>(message),
                    cancellationToken: cancellationToken);
            }
        }
        
        public NotifyMessageAddedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) : 
            base(hubContextProvider, context, mapper)
        {}
    }
}
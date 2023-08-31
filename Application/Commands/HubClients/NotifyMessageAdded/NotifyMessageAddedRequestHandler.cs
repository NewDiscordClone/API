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
            Context.SetToken(cancellationToken);
            
            Message message = await Context.Messages.FindAsync(request.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);
            
            IEnumerable<string> connectedUsers = GetConnections(chat);

            foreach (string connectionId in connectedUsers)
            {
                await Clients.Client(connectionId).SendAsync("MessageAdded", Mapper.Map<Message>(message),
                    cancellationToken: cancellationToken);
            }
        }
        
        public NotifyMessageAddedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) : 
            base(hubContextProvider, context, mapper)
        {}
    }
}
using Application.Application;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Commands.HubClients.Messages.MessageAdded
{
    public class NotifyMessageAddedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyMessageAddedRequest>
    {
        public async Task Handle(NotifyMessageAddedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            
            Message message = await Context.Messages.FindAsync(request.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);
            
            await SendAsync(ClientMethods.MessageAdded, message, GetConnections(chat));
        }
        
        public NotifyMessageAddedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) : 
            base(hubContextProvider, context, mapper)
        {}
    }
}
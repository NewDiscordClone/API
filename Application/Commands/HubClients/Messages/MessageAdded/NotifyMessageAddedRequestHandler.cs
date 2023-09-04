using Application.Application;
using Application.Interfaces;
using Application.Models;
using Application.Models.LookUps;
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
            MessageDto messageDto = Mapper.Map<MessageDto>(message);
            messageDto.User = Mapper.Map<UserLookUp>(await Context.SqlUsers.FindAsync(message.User));
            Chat chat = await Context.Chats.FindAsync(message.ChatId);
            
            await SendAsync(ClientMethods.MessageAdded, messageDto, GetConnections(chat));
        }
        
        public NotifyMessageAddedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) : 
            base(hubContextProvider, context, mapper)
        {}
    }
}
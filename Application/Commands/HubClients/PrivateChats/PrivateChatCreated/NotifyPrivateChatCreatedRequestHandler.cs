using Application.Application;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Commands.HubClients.PrivateChats.PrivateChatCreated
{
    public class NotifyPrivateChatCreatedRequestHandler : HubRequestHandlerBase, IRequestHandler<NotifyPrivateChatCreatedRequest>
    {
        public NotifyPrivateChatCreatedRequestHandler(IHubContextProvider hubContextProvider, IAppDbContext context, IMapper mapper) : base(hubContextProvider, context, mapper)
        {
        }

        public async Task Handle(NotifyPrivateChatCreatedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Chat chat = await Context.PersonalChats.FindAsync(request.ChatId);

            PrivateChatLookUp lookUp = chat switch
            {
                GroupChat gChat => Mapper.Map<PrivateChatLookUp>(gChat),
                PersonalChat pChat => new PrivateChatLookUp(pChat, UserId),
                _ => throw new ArgumentException("the given chat is not an private chat")
            };

            await SendAsync(ClientMethods.PrivateChatCreated, lookUp, GetConnections(chat));
        }
    }
}
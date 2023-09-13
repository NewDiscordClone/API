using Application.Application;
using Application.Common.Interfaces;
using Application.HubClients;
using Application.Models;
using Application.Models.LookUps;
using AutoMapper;
using MediatR;

namespace Application.HubClients.PrivateChats.PrivateChatCreated
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
            User user = await Context.SqlUsers.FindAsync(UserId);

            PrivateChatLookUp lookUp = chat switch
            {
                GroupChat gChat => Mapper.Map<PrivateChatLookUp>(gChat),
                PersonalChat pChat => new PrivateChatLookUp(pChat, Mapper.Map<UserLookUp>(user)),
                _ => throw new ArgumentException("the given chat is not an private chat")
            };

            await SendAsync(ClientMethods.PrivateChatCreated, lookUp, GetConnections(chat));
        }
    }
}
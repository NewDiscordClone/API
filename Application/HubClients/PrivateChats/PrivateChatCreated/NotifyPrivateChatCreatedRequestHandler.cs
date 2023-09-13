using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatCreated
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
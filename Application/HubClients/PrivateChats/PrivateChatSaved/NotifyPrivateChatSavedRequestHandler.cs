using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatSaved
{
    public class NotifyPrivateChatSavedRequestHandler : HubRequestHandlerBase,
        IRequestHandler<NotifyPrivateChatSavedRequest>
    {
        public NotifyPrivateChatSavedRequestHandler(
            IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper)
            : base(hubContextProvider, context, userProvider, mapper)
        {
        }

        public async Task Handle(NotifyPrivateChatSavedRequest request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Chat chat = await Context.PersonalChats.FindAsync(request.ChatId);
            foreach (var user in chat.Users)
            {
                await SendToUser(chat, user);
            }
        }

        private async Task SendToUser(Chat chat, Guid userId)
        {
            PrivateChatLookUp lookUp;
            switch(chat)
            {
                case GroupChat gChat:
                    lookUp = Mapper.Map<PrivateChatLookUp>(gChat);
                    break; 
                case PersonalChat pChat:
                    User other = await Context.SqlUsers.FindAsync(chat.Users.First(u => u != userId));
                    lookUp = new PrivateChatLookUp(pChat, Mapper.Map<UserLookUp>(other));
                    break;
                default:
                    throw new ArgumentException("the given chat is not an private chat");
            };
            await SendAsync(ClientMethods.PrivateChatSaved, lookUp, GetConnections(userId));
        }
    }
}
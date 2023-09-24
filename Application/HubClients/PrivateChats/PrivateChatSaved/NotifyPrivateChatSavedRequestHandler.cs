using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatSaved
{
    public class NotifyPrivateChatSavedQueryHandler : HubRequestHandlerBase,
        IRequestHandler<NotifyPrivateChatSavedQuery>
    {
        public NotifyPrivateChatSavedQueryHandler(
            IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper)
            : base(hubContextProvider, context, userProvider, mapper)
        {
        }

        public async Task Handle(NotifyPrivateChatSavedQuery request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Chat chat = await Context.PersonalChats.FindAsync(request.ChatId);
            foreach (Guid profile in chat.Profiles)
            {
                Guid userId = Context.UserProfiles.Find(profile)!.UserId;
                await SendToUser(chat, userId);
            }
        }

        private async Task SendToUser(Chat chat, Guid userId)
        {
            PrivateChatLookUp lookUp;
            switch (chat)
            {
                case GroupChat gChat:
                    lookUp = Mapper.Map<PrivateChatLookUp>(gChat);
                    break;
                case PersonalChat pChat:
                    User other = await Context.Users
                        .SingleAsync(user => user.UserProfiles
                        .Any(profile => profile.ChatId == chat.Id && user.Id != UserId));

                    lookUp = new PrivateChatLookUp(pChat, Mapper.Map<UserLookUp>(other));
                    break;
                default:
                    throw new ArgumentException("the given chat is not an private chat");
            };
            await SendAsync(ClientMethods.PrivateChatSaved, lookUp, GetConnections(userId));
        }
    }
}
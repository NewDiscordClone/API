using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatSaved
{
    public class NotifyPrivateChatSavedQueryHandler : HubHandler,
        IRequestHandler<NotifyPrivateChatSavedQuery>
    {
        private readonly IConvertor _convertor;

        public NotifyPrivateChatSavedQueryHandler(IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IConvertor convertor)
            : base(hubContextProvider, context, userProvider, mapper)
        {
            _convertor = convertor;
        }

        public async Task Handle(NotifyPrivateChatSavedQuery query, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);

            List<Guid> userIds = await Context.UserProfiles
                .Where(profile => profile.ChatId == query.Chat.Id)!
                .Select(profile => profile.UserId)
                .ToListAsync(cancellationToken);

            await SendToUser(query.Chat, userIds, cancellationToken);

        }

        private async Task SendToUser(Chat chat, List<Guid> userIds, CancellationToken cancellationToken)
        {
            PrivateChatLookUp lookUp;
            switch (chat)
            {
                case GroupChat gChat:
                    lookUp = Mapper.Map<GroupChatLookup>(gChat);
                    break;
                case PersonalChat pChat:
                    User other = await Context.Users
                        .SingleAsync(user => user.UserProfiles
                        .Any(profile => profile.ChatId == chat.Id && user.Id != UserId),
                        cancellationToken: cancellationToken);

                    lookUp = Mapper.Map<PersonalChatLookup>((other, pChat));
                    break;
                default:
                    throw new ArgumentException("the given chat is not an private chat");
            };

            lookUp.Title ??= await _convertor.FillChatTitleAsync(userIds, cancellationToken);
            await SendAsync(ClientMethods.PrivateChatSaved, lookUp, GetConnections(userIds));
        }
    }
}
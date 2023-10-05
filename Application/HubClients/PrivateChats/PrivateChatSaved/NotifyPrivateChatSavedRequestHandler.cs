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

        public async Task Handle(NotifyPrivateChatSavedQuery request, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Chat? chat = await Context.PersonalChats.FindOrDefaultAsync(request.ChatId,
                cancellationToken);

            if (chat == null)
                return;

            List<Guid> userIds = await Context.UserProfiles
                .Where(profile => profile.ChatId == chat.Id)!
                .Select(profile => profile.UserId)
                .ToListAsync(cancellationToken);

            await SendToUser(chat, userIds, cancellationToken);

        }

        private async Task SendToUser(Chat chat, List<Guid> userIds, CancellationToken cancellationToken)
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
                        .Any(profile => profile.ChatId == chat.Id && user.Id != UserId),
                        cancellationToken: cancellationToken);

                    lookUp = new PrivateChatLookUp(pChat, other);
                    break;
                default:
                    throw new ArgumentException("the given chat is not an private chat");
            };

            lookUp.Title ??= await _convertor.FillChatTitleAsync(userIds, cancellationToken);
            await SendAsync(ClientMethods.PrivateChatSaved, lookUp, GetConnections(userIds));
        }
    }
}
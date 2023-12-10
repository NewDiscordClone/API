using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;
using Sparkle.Domain.LookUps;

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
            switch (chat)
            {
                case GroupChat gChat:
                    await SendToGroupChat(gChat, userIds, cancellationToken);
                    break;
                case PersonalChat pChat:
                    await SendToPersonalChat(userIds, pChat, cancellationToken);
                    break;
                default:
                    throw new ArgumentException("the given chat is not an private chat");
            };
        }

        private async Task SendToPersonalChat(List<Guid> userIds, PersonalChat pChat, CancellationToken cancellationToken)
        {
            PrivateChatLookUp lookUp1 = await GetOtherDto(pChat, UserId, cancellationToken);

            Guid otherUserId = userIds
                .Except(new Guid[] { UserId })
                .Single();
            PrivateChatLookUp lookUp2 = await GetOtherDto(pChat, otherUserId, cancellationToken);

            lookUp1.Title ??= await _convertor.FillChatTitleAsync(userIds, UserId, cancellationToken);
            lookUp1.Title ??= await _convertor.FillChatTitleAsync(userIds, otherUserId, cancellationToken);

            await SendAsync(ClientMethods.PrivateChatSaved, lookUp1, GetConnections(otherUserId));
            await SendAsync(ClientMethods.PrivateChatSaved, lookUp2, GetConnections(UserId));
        }

        private async Task SendToGroupChat(GroupChat chat, List<Guid> userIds, CancellationToken cancellationToken)
        {
            GroupChatLookup lookup = Mapper.Map<GroupChatLookup>(chat);

            lookup.Title ??= await _convertor.FillChatTitleAsync(userIds, cancellationToken);

            await SendAsync(ClientMethods.PrivateChatSaved, lookup, userIds);
        }

        private async Task<PrivateChatLookUp> GetOtherDto(PersonalChat chat, Guid resiverId, CancellationToken cancellationToken)
        {
            User other = await Context.Users
                .SingleAsync(user => user.UserProfiles
                .Any(profile => profile.ChatId == chat.Id && user.Id != resiverId),
                cancellationToken: cancellationToken);

            return Mapper.Map<PersonalChatLookup>((other, chat));
        }
    }
}
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.PrivateChats
{
    public class NotifyPersonalChatSaved : HubHandler,
        INotificationHandler<PersonalChatCreated>, INotificationHandler<PersonalChatUpdated>
    {
        private readonly IConvertor _convertor;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IUserRepository _userRepository;

        private async Task Handle(PersonalChat chat, CancellationToken cancellationToken)
        {
            List<Guid> userIds = await _profileRepository.ExecuteCustomQuery(profiles => profiles
                .Where(profile => profile.ChatId == chat.Id)
                .Select(profile => profile.UserId))
                .ToListAsync(cancellationToken);

            await SendToUser(chat, userIds, cancellationToken);
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
                    User other = await _userRepository.ExecuteCustomQuery(users => users
                        .Where(user => user.UserProfiles
                        .Any(profile => profile.ChatId == chat.Id && user.Id != UserId)))
                        .SingleAsync(cancellationToken);

                    lookUp = Mapper.Map<PersonalChatLookup>((other, pChat));
                    break;
                default:
                    throw new ArgumentException("the given chat is not an private chat");
            };

            lookUp.Title ??= await _convertor.FillChatTitleAsync(userIds, cancellationToken);

            IEnumerable<string> connections = await ConnectionsRepository
                .FindConnectionsAsync(userIds, cancellationToken);

            await SendAsync(ClientMethods.PrivateChatSaved, lookUp, connections, cancellationToken);
        }

        public async Task Handle(PersonalChatCreated notification, CancellationToken cancellationToken)
        {
            PersonalChat chat = notification.Chat;
            await Handle(chat, cancellationToken);
        }

        public async Task Handle(PersonalChatUpdated notification, CancellationToken cancellationToken)
        {
            PersonalChat chat = notification.Chat;
            await Handle(chat, cancellationToken);
        }

        public NotifyPersonalChatSaved(IHubContextProvider hubContextProvider,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IConvertor convertor,
            IConnectionsRepository connectionsRepository,
            IUserProfileRepository profileRepository)
            : base(hubContextProvider, userProvider, mapper, connectionsRepository)
        {
            _convertor = convertor;
            _profileRepository = profileRepository;
        }
    }
}
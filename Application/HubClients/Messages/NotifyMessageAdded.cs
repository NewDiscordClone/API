using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.HubClients.Common;
using Sparkle.Application.Models;
using Sparkle.Application.Models.Events;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Messages
{
    public class NotifyMessageAdded : HubHandler, INotificationHandler<MessageSentEvent>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public async Task Handle(MessageSentEvent notification, CancellationToken cancellationToken)
        {
            Message message = notification.Message;
            MessageDto messageDto = Mapper.Map<MessageDto>(message);

            User author = await _userRepository.FindAsync(message.Author, cancellationToken);
            UserProfile authorProfile = await _userProfileRepository
                .FindAsync(message.AuthorProfile, cancellationToken);

            UserViewModel user = Mapper.Map<UserViewModel>(author);

            if (authorProfile is ServerProfile serverProfile && serverProfile.DisplayName is not null)
                user.DisplayName = serverProfile.DisplayName;

            Chat chat = await _chatRepository.FindAsync(messageDto.ChatId, cancellationToken);
            if (chat is Channel channel)
                messageDto.ServerId = channel.ServerId;

            IEnumerable<string> connections = await ConnectionsRepository
                .FindConnectionsAsync(chat, cancellationToken);

            await SendAsync(ClientMethods.MessageAdded, messageDto, connections, cancellationToken);
        }

        public NotifyMessageAdded(IHubContextProvider hubContextProvider,
            IMapper mapper,
            IConnectionsRepository connectionsRepository,
            IChatRepository chatRepository,
            IUserRepository userRepository,
            IUserProfileRepository userProfileRepository) : base(hubContextProvider, mapper, connectionsRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }
    }
}
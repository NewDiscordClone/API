using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Messages.Queries.GetPinnedMessages
{
    public class GetPinnedMessagesQueryHandler : RequestHandler,
        IRequestHandler<GetPinnedMessagesQuery, List<MessageDto>>
    {

        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;

        public async Task<List<MessageDto>> Handle(GetPinnedMessagesQuery query, CancellationToken cancellationToken)
        {
            Chat chat = await _chatRepository.FindAsync(query.ChatId, cancellationToken);

            List<Message> messages = await _messageRepository.ExecuteCustomQuery(messages => messages
                .Where(message => message.ChatId == query.ChatId
                 && message.IsPinned))
                .ToListAsync(cancellationToken);

            List<MessageDto> dtos = messages.ConvertAll(Mapper.Map<MessageDto>);

            for (int i = 0; i < messages.Count; i++)
            {
                Message message = messages[i];
                MessageDto dto = dtos[i];

                UserProfile profile = await _userProfileRepository.FindAsync(message.AuthorProfile, cancellationToken);
                User user = await _userRepository.FindAsync(profile.UserId, cancellationToken);

                dto.Author = Mapper.Map<UserViewModel>(user);

                if (profile is ServerProfile serverProfile && chat is Channel channel)
                {
                    dto.Author.DisplayName = serverProfile.DisplayName ?? dto.Author.DisplayName;
                    dto.ServerId = channel.ServerId;
                }
            };

            return dtos;
        }


        public GetPinnedMessagesQueryHandler(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IUserProfileRepository userProfileRepository,
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IUserRepository userRepository) : base(userProvider, mapper)
        {
            _userProfileRepository = userProfileRepository;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }
    }
}
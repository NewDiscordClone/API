using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Messages.Queries.GetMessages
{
    public class GetMessagesQueryHandler : RequestHandlerBase, IRequestHandler<GetMessagesQuery, List<MessageDto>>
    {
        private const int _pageSize = 50;

        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _profileRepository;

        public async Task<List<MessageDto>> Handle(GetMessagesQuery query, CancellationToken cancellationToken)
        {
            List<MessageDto> result = [];

            foreach (Message message in await _messageRepository.GetMessagesInChatAsync(query.ChatId, query.MessagesCount, _pageSize))
            {
                MessageDto dto = Mapper.Map<MessageDto>(message);

                UserProfile? userProfile = await _profileRepository
                    .FindAsync(message.AuthorProfile, cancellationToken);

                User? user = await _userRepository.FindAsync(userProfile.UserId, cancellationToken);

                dto.Author = Mapper.Map<UserViewModel>(user);

                if (userProfile is not null and ServerProfile serverProfile)
                {
                    dto.Author.DisplayName = serverProfile.DisplayName
                        ?? dto.Author.DisplayName;
                }

                result.Add(dto);
            }
            return result;
        }

        public GetMessagesQueryHandler(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IMessageRepository messageRepository,
            IUserProfileRepository profileRepository,
            IUserRepository userRepository) :
            base(userProvider, mapper)
        {
            _messageRepository = messageRepository;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
        }
    }
}
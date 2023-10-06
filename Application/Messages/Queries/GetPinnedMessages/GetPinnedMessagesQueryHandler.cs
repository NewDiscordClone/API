using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Messages.Queries.GetPinnedMessages
{
    public class GetPinnedMessagesQueryHandler : RequestHandlerBase,
        IRequestHandler<GetPinnedMessagesQuery, List<MessageDto>>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public async Task<List<MessageDto>> Handle(GetPinnedMessagesQuery query,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Chat chat = await Context.Chats.FindAsync(query.ChatId, cancellationToken);

            List<Message> messages = await Context.GetPinnedMessagesAsync(chat.Id);
            List<MessageDto> dtos = messages.ConvertAll(Mapper.Map<MessageDto>);

            for (int i = 0; i < messages.Count; i++)
            {
                Message message = messages[i];
                MessageDto dto = dtos[i];

                UserProfile profile = await _userProfileRepository.FindAsync(message.AuthorProfile, cancellationToken);
                User user = await Context.Users.FindAsync(new object?[] { profile.UserId }, cancellationToken: cancellationToken)
                    ?? throw new EntityNotFoundException(profile.UserId);

                dto.Author = Mapper.Map<UserViewModel>(user);

                if (profile is ServerProfile serverProfile && chat is Channel channel)
                {
                    dto.Author.DisplayName = serverProfile.DisplayName ?? dto.Author.DisplayName;
                    dto.ServerId = channel.ServerId;
                }
            };

            return dtos;
        }


        public GetPinnedMessagesQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper, IUserProfileRepository userProfileRepository) : base(
            context, userProvider, mapper)
        {
            _userProfileRepository = userProfileRepository;
        }
    }
}
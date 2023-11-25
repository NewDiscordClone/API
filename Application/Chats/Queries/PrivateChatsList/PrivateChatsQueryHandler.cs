using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Chats.Queries.PrivateChatsList
{
    public class PrivateChatsQueryHandler : RequestHandler,
        IRequestHandler<PrivateChatsQuery, List<PrivateChatLookUp>>
    {
        private readonly IConvertor _convertor;
        private readonly IChatRepository _chatRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public PrivateChatsQueryHandler(
            IAuthorizedUserProvider userProvider,
            IConvertor convertor,
            IUserProfileRepository userProfileRepository)
            : base(userProvider)
        {
            _convertor = convertor;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<List<PrivateChatLookUp>> Handle(PrivateChatsQuery query, CancellationToken cancellationToken)
        {
            List<string?> chatIds = await _userProfileRepository.ExecuteCustomQuery(profiles =>
                profiles.Where(profile => profile.UserId == UserId && profile.ChatId != null)
                .Select(profile => profile.ChatId))
                .ToListAsync(cancellationToken);

            List<PersonalChat> chats = await _chatRepository.ExecuteCustomQuery
                (chats => chats
                    .Where(chat => chatIds.Contains(chat.Id))
                    .OfType<PersonalChat>())
                .ToListAsync(cancellationToken);

            List<PrivateChatLookUp> chatDtos = chats.ConvertAll(_convertor.Convert);

            return chatDtos;
        }
    }
}
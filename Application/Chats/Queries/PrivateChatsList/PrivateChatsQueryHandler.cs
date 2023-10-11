using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Chats.Queries.PrivateChatsList
{
    public class PrivateChatsQueryHandler : RequestHandlerBase,
        IRequestHandler<PrivateChatsQuery, List<PrivateChatLookUp>>
    {
        private readonly IConvertor _convertor;

        public PrivateChatsQueryHandler(
            IAppDbContext appDbContext,
            IAuthorizedUserProvider userProvider,
            IConvertor convertor)
            : base(appDbContext, userProvider)
        {
            _convertor = convertor;
        }

        public async Task<List<PrivateChatLookUp>> Handle(PrivateChatsQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            List<string?> chatIds = await Context.UserProfiles.
                Where(profile => profile.UserId == UserId && profile.ChatId != null)
                .Select(profile => profile.ChatId)
                .ToListAsync(cancellationToken);

            List<PersonalChat> chats = await Context.PersonalChats
                .FilterAsync(chat => chatIds.Contains(chat.Id), cancellationToken);

            List<PrivateChatLookUp> chatDtos = chats.ConvertAll(_convertor.Convert);

            return chatDtos;
        }
    }
}
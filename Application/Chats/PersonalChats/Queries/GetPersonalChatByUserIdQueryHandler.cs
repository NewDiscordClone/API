using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Chats.Queries.PrivateChatDetails;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.PersonalChats.Queries
{
    public class GetPersonalChatByUserIdQueryHandler : RequestHandlerBase,
        IRequestHandler<GetPersonalChatByUserIdQuery, PrivateChatViewModel>
    {
        private readonly IConvertor _convertor;
        public GetPersonalChatByUserIdQueryHandler(IAppDbContext context, IConvertor convertor) : base(context)
        {
            _convertor = convertor;
        }

        public async Task<PrivateChatViewModel> Handle(GetPersonalChatByUserIdQuery query, CancellationToken cancellationToken)
        {
            var userProfiles = Context.Users
                .Where(user => user.Id == UserId)
                .SelectMany(user => user.UserProfiles.Select(p => new { p.Id, p.ChatId }));

            List<Guid> profilesWithSameChats = await Context.UserProfiles
                .Where(profile => profile.UserId == query.UserId)
                .Where(profile => userProfiles.Any(p => p.ChatId == profile.ChatId))
                .Select(profile => profile.Id)
                .ToListAsync(cancellationToken);

            PersonalChat chat = await Context.PersonalChats
                .SingleAsync(chat => chat.Profiles.Count == 2
                && chat.Profiles.Any(id => userProfiles.Select(x => x.Id).Contains(id)
                && chat.Profiles.Any(id => profilesWithSameChats.Contains(id))), cancellationToken);

            return await _convertor.ConvertToViewModelAsync(chat, cancellationToken);
        }
    }
}

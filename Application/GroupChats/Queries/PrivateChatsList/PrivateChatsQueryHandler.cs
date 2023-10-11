using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.GroupChats.Queries.PrivateChatsList
{
    public class PrivateChatsQueryHandler : RequestHandlerBase,
        IRequestHandler<PrivateChatsQuery, List<PrivateChatLookUp>>
    {

        private readonly Common.Interfaces.Repositories.IUserProfileRepository _userRepository;

        public PrivateChatsQueryHandler(
            IAppDbContext appDbContext,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            Common.Interfaces.Repositories.IUserProfileRepository userRepository)
            : base(appDbContext, userProvider, mapper)
        {
            _userRepository = userRepository;
        }

        public async Task<List<PrivateChatLookUp>> Handle(PrivateChatsQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            List<PrivateChatLookUp> chatDtos = new();

            List<string?> chatIds = await Context.UserProfiles.
                Where(profile => profile.UserId == UserId && profile.ChatId != null)
                .Select(profile => profile.ChatId)
                .ToListAsync(cancellationToken);

            List<PersonalChat> chats = await Context.PersonalChats
                .FilterAsync(chat => chatIds.Contains(chat.Id), cancellationToken);

            foreach (PersonalChat chat in chats)
            {
                if (chat is GroupChat groupChat)
                {
                    PrivateChatLookUp lookUp = Mapper.Map<PrivateChatLookUp>(groupChat);

                    List<Guid> userIds = await Context.UserProfiles
                        .Where(profile => profile.ChatId == groupChat.Id)
                        .Select(profile => profile.UserId)
                        .ToListAsync(cancellationToken);

                    if (string.IsNullOrWhiteSpace(groupChat.Title))
                    {
                        List<string?> userDisplayNames = await Context.Users
                            .Where(user => user.Id != UserId && userIds.Contains(user.Id))
                            .Select(u => u.DisplayName ?? u.UserName)
                            .ToListAsync(cancellationToken);

                        lookUp.Title = string.Join(", ", userDisplayNames);
                    }

                    chatDtos.Add(lookUp);
                }
                else
                {
                    Guid otherUserId = await Context.UserProfiles
                        .Where(profile => profile.ChatId == chat.Id && profile.UserId != UserId)
                        .Select(profile => profile.UserId)
                        .SingleAsync(cancellationToken);

                    User? otherUser = await Context.Users.FindAsync(new object[] { otherUserId }, cancellationToken: cancellationToken);
                    chatDtos.Add(new PrivateChatLookUp(chat, Mapper.Map<UserLookUp>(otherUser)));
                }
            }

            return chatDtos;
        }

    }
}
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.GroupChats.Queries.PrivateChatsList
{
    public class PrivateChatsQueryHandler : RequestHandlerBase,
        IRequestHandler<PrivateChatsQuery, List<PrivateChatLookUp>>
    {

        private readonly IUserProfileRepository _userRepository;

        public PrivateChatsQueryHandler(
            IAppDbContext appDbContext,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IUserProfileRepository userRepository)
            : base(appDbContext, userProvider, mapper)
        {
            _userRepository = userRepository;
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


            List<PrivateChatLookUp> chatDtos = new();
            foreach (PersonalChat chat in chats)
            {
                PrivateChatLookUp lookUp = chat switch
                {
                    GroupChat groupChat => await GetDtoFromGroupChat(groupChat, cancellationToken),
                    PersonalChat => await GetDtoFromPersonalChat(chat, cancellationToken)
                };

                chatDtos.Add(lookUp);
            }

            return chatDtos;
        }

        private async Task<PrivateChatLookUp> GetDtoFromPersonalChat(PersonalChat chat, CancellationToken cancellationToken)
        {
            Guid otherUserId = await Context.UserProfiles
                .Where(profile => profile.ChatId == chat.Id && profile.UserId != UserId)
                .Select(profile => profile.UserId)
                .SingleAsync(cancellationToken);

            User? otherUser = await Context.Users.FindAsync(new object[] { otherUserId },
                cancellationToken: cancellationToken)
                ?? throw new EntityNotFoundException(message: $"User {otherUserId} not found", otherUserId);

            return new(chat, otherUser);
        }

        private async Task<PrivateChatLookUp> GetDtoFromGroupChat(GroupChat groupChat, CancellationToken cancellationToken)
        {
            PrivateChatLookUp lookUp = Mapper.Map<PrivateChatLookUp>(groupChat);

            List<Guid> userIds = await Context.UserProfiles
                .Where(profile => profile.ChatId == groupChat.Id)
                .Select(profile => profile.UserId)
                .ToListAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(groupChat.Title))
            {
                List<string> userDisplayNames = await Context.Users
                    .Where(user => user.Id != UserId && userIds.Contains(user.Id))
                    .Select(u => u.DisplayName ?? u.UserName!)
                    .ToListAsync(cancellationToken);

                lookUp.Title = string.Join(", ", userDisplayNames);
            }

            return lookUp;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Common.Convertor
{
    public partial class Convertor : IConvertor
    {
        protected Guid UserId => _userProvider.GetUserId();
        public PrivateChatLookUp Convert(PersonalChat chat)
        {
            return ConvertAsync(chat).Result;
        }

        public Task<PrivateChatLookUp> ConvertAsync(PersonalChat chat, CancellationToken cancellationToken = default)
        {
            return chat switch
            {
                GroupChat groupChat => GetDtoFromGroupChat(groupChat, cancellationToken),
                PersonalChat personalChat => GetDtoFromPersonalChat(personalChat, cancellationToken)
            };
        }

        private async Task<PrivateChatLookUp> GetDtoFromPersonalChat(PersonalChat chat, CancellationToken cancellationToken)
        {
            Guid otherUserId = chat.Profiles.Single(id => id != UserId);

            User? otherUser = await _context.Users.FindAsync(new object[] { otherUserId },
                cancellationToken: cancellationToken)
                ?? throw new EntityNotFoundException(message: $"User {otherUserId} not found", otherUserId);

            return new(chat, otherUser);
        }

        private async Task<PrivateChatLookUp> GetDtoFromGroupChat(GroupChat groupChat, CancellationToken cancellationToken)
        {
            PrivateChatLookUp lookUp = _mapper.Map<PrivateChatLookUp>(groupChat);

            List<Guid> userIds = await _context.UserProfiles
                .Where(profile => profile.ChatId == groupChat.Id)
                .Select(profile => profile.UserId)
                .ToListAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(groupChat.Title))
            {
                lookUp.Title = await FillChatTitle(userIds, cancellationToken);
            }

            return lookUp;
        }

        public async Task<string> FillChatTitle(List<Guid> userIds, CancellationToken cancellationToken)
        {
            List<string> userDisplayNames = await _context.Users
                .Where(user => user.Id != UserId && userIds.Contains(user.Id))
                .Select(u => u.DisplayName ?? u.UserName!)
                .ToListAsync(cancellationToken);

            return string.Join(", ", userDisplayNames);
        }
    }
}

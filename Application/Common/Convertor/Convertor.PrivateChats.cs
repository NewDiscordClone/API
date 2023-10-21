using AutoMapper.QueryableExtensions;
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

        public async Task<PrivateChatLookUp> ConvertAsync(PersonalChat chat, CancellationToken cancellationToken = default)
        {
            return chat switch
            {
                GroupChat groupChat => await GetDtoFromGroupChat(groupChat, cancellationToken),
                PersonalChat personalChat => await GetDtoFromPersonalChat(personalChat, cancellationToken)
            };
        }

        private async Task<PersonalChatLookup> GetDtoFromPersonalChat(PersonalChat chat, CancellationToken cancellationToken)
        {
            Guid otherUserId = await _context.UserProfiles
                .Where(profile => profile.ChatId == chat.Id && profile.UserId != UserId)
                .Select(profile => profile.UserId)
                .SingleAsync(cancellationToken);


            User? otherUser = await _context.Users.FindAsync(new object[] { otherUserId },
                cancellationToken: cancellationToken)
                ?? throw new EntityNotFoundException(message: $"User {otherUserId} not found", otherUserId);

            return _mapper.Map<(User User, PersonalChat Chat), PersonalChatLookup>((otherUser, chat));
        }

        private async Task<GroupChatLookup> GetDtoFromGroupChat(GroupChat groupChat, CancellationToken cancellationToken)
        {
            GroupChatLookup lookUp = _mapper.Map<GroupChatLookup>(groupChat);

            List<Guid> userIds = await _context.UserProfiles
                .Where(profile => profile.ChatId == groupChat.Id)
                .Select(profile => profile.UserId)
                .ToListAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(groupChat.Title))
            {
                lookUp.Title = await FillChatTitleAsync(userIds, cancellationToken);
            }

            return lookUp;
        }

        public async Task<string> FillChatTitleAsync(List<Guid> userIds, CancellationToken cancellationToken)
        {
            List<string> userDisplayNames = await _context.Users
                .Where(user => user.Id != UserId && userIds.Contains(user.Id))
                .Select(u => u.DisplayName ?? u.UserName!)
                .ToListAsync(cancellationToken);

            return string.Join(", ", userDisplayNames);
        }

        public async Task<PrivateChatViewModel> ConvertToViewModelAsync(PersonalChat chat, CancellationToken cancellationToken = default)
        {
            PrivateChatViewModel viewModel = _mapper.Map<PrivateChatViewModel>(chat);

            List<UserProfileViewModel> profiles = await _context.UserProfiles
                .Where(profile => profile.ChatId == chat.Id)
                .ProjectTo<UserProfileViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            foreach (UserProfileViewModel profile in profiles)
            {
                User user = await _context.Users
                    .Where(u => u.Id == profile.UserId)
                    .SingleAsync(cancellationToken);

                profile.Name = user.DisplayName ?? user.UserName;
                profile.AvatarUrl = user.Avatar;
                profile.Status = user.Status;
                profile.TextStatus = user.TextStatus;

                viewModel.Profiles.Add(profile);
            }

            if (string.IsNullOrEmpty(viewModel.Title))
            {
                List<Guid> userIds = viewModel.Profiles
                    .Where(profile => profile.UserId != UserId)
                    .Select(profile => profile.UserId)
                    .ToList();

                viewModel.Title = await FillChatTitleAsync(userIds, cancellationToken);
            }

            if (chat is GroupChat groupChat)
            {
                viewModel.OwnerId = groupChat.OwnerId;
            }

            return viewModel;
        }
    }
}

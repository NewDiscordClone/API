using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.Queries.PrivateChatDetails
{
    public class PrivateChatDetailsQueryHandler : RequestHandlerBase, IRequestHandler<PrivateChatDetailsQuery, PrivateChatViewModel>
    {
        public PrivateChatDetailsQueryHandler(IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper)
            : base(context, userProvider, mapper)
        {
        }

        public async Task<PrivateChatViewModel> Handle(PrivateChatDetailsQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            PersonalChat chat = await Context.PersonalChats.FindAsync(query.ChatId, cancellationToken);

            PrivateChatViewModel viewModel = Mapper.Map<PrivateChatViewModel>(chat);

            List<UserProfileViewModel> profiles = await Context.UserProfiles
                .Where(profile => profile.ChatId == query.ChatId)
                .ProjectTo<UserProfileViewModel>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            foreach (UserProfileViewModel profile in profiles)
            {
                var user = await Context.Users
                    .Where(u => u.Id == profile.UserId)
                    .Select(user => user.DisplayName != null
                    ? new { Name = user.DisplayName, AvatarUrl = user.Avatar }
                    : new { Name = user.UserName, AvatarUrl = user.Avatar })
                    .SingleAsync(cancellationToken);

                profile.Name = user.Name;
                profile.AvatarUrl = user.AvatarUrl;

                viewModel.Profiles.Add(profile);
            }

            if (string.IsNullOrEmpty(viewModel.Title))
            {
                List<string> userNames = viewModel.Profiles
                    .Where(profile => profile.UserId != UserId)
                    .Select(profile => profile.Name)
                    .ToList();

                viewModel.Title = string.Join(", ", userNames);
            }

            if (chat is GroupChat groupChat)
            {
                viewModel.OwnerId = groupChat.OwnerId;
            }

            return viewModel;
        }

    }
}
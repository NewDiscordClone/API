using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Options;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.GroupChats.Commands.CreateGroupChat
{
    public class CreateGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<CreateGroupChatCommand, GroupChat>
    {
        private readonly Common.Interfaces.Repositories.IUserProfileRepository _userProfileRepository;
        private readonly IRoleFactory _roleFactory;
        private readonly string _apiUri;

        public async Task<GroupChat> Handle(CreateGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = new()
            {
                Title = command.Title,
                Image = command.Image ?? GetDefaultImage(),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            List<UserProfile> profiles = command.UserIds.ConvertAll(id => new UserProfile()
            {
                UserId = id,
                ChatId = chat.Id,
                Roles = { _roleFactory.GroupChatMemberRole }
            });

            UserProfile ownerProfile = new()
            {
                UserId = UserId,
                ChatId = chat.Id,
                Roles = { _roleFactory.GroupChatOwnerRole }
            };

            profiles.Add(ownerProfile);
            chat.Profiles = profiles.ConvertAll(p => p.Id);
            chat.OwnerId = ownerProfile.Id;

            await _userProfileRepository.AddManyAsync(profiles, cancellationToken);
            await Context.GroupChats.AddAsync(chat, cancellationToken);

            return chat;
        }

        private string GetDefaultImage()
        {
            Random random = new();
            string[] defaultImageIds = Constants.Chats.DefaultImages;
            int randomIndex = random.Next(0, defaultImageIds.Length);
            string imageId = defaultImageIds[randomIndex].ToString();

            return _apiUri + $"/api/media/{imageId}.png";
        }

        public CreateGroupChatCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper, Common.Interfaces.Repositories.IUserProfileRepository userProfileRepository, IRoleFactory roleFactory, IOptions<ApiOptions> options)
            : base(context, userProvider, mapper)
        {
            _userProfileRepository = userProfileRepository;
            _roleFactory = roleFactory;
            _apiUri = options.Value.ApiUri;
        }
    }
}
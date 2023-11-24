using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.GroupChats.Commands.CreateGroupChat
{
    public class CreateGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<CreateGroupChatCommand, GroupChat>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IRoleFactory _roleFactory;
        private readonly IChatRepository _chatRepository;
        public async Task<GroupChat> Handle(CreateGroupChatCommand command, CancellationToken cancellationToken)
        {
            GroupChat chat = new()
            {
                Title = command.Title,
                Image = command.Image,
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
            await _chatRepository.AddAsync(chat, cancellationToken);

            return chat;
        }

        public CreateGroupChatCommandHandler(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IUserProfileRepository userProfileRepository,
            IRoleFactory roleFactory,
            IChatRepository chatRepository) : base(userProvider, mapper)
        {
            _userProfileRepository = userProfileRepository;
            _roleFactory = roleFactory;
            _chatRepository = chatRepository;
        }
    }
}
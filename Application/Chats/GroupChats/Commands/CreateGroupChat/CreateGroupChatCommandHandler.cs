using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.CreateGroupChat
{
    public class CreateGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<CreateGroupChatCommand, string>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IRoleFactory _roleFactory;
        public async Task<string> Handle(CreateGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = new()
            {
                Title = command.Title,
                Image = command.Image,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            List<UserProfile> profiles = command.UsersId.ConvertAll(id => new UserProfile()
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
            return chat.Id;
        }

        public CreateGroupChatCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper, IUserProfileRepository userProfileRepository, IRoleFactory roleFactory) : base(context, userProvider, mapper)
        {
            _userProfileRepository = userProfileRepository;
            _roleFactory = roleFactory;
        }
    }
}
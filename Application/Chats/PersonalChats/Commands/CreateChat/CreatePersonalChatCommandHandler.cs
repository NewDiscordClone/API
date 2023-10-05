using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.PersonalChats.Commands.CreateChat
{
    public class CreatePersonalChatCommandHandler : RequestHandlerBase, IRequestHandler<CreatePersonalChatCommand, PersonalChat>
    {
        private readonly IRoleFactory _roleFactory;
        private readonly IUserProfileRepository _userProfileRepository;
        public CreatePersonalChatCommandHandler(IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IUserProfileRepository userProfileRepository)
            : base(context, userProvider)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<PersonalChat> Handle(CreatePersonalChatCommand command, CancellationToken cancellationToken)
        {
            PersonalChat chat = new()
            {
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };

            List<UserProfile> profiles = new()
            {
                new UserProfile
                {
                    UserId = command.UserId,
                    Roles = { _roleFactory.PersonalChatMemberRole },
                    ChatId = chat.Id
                },
                new UserProfile
                {
                    UserId = UserId,
                    Roles = { _roleFactory.PersonalChatMemberRole },
                    ChatId = chat.Id
                }
            };

            chat.Profiles = profiles.Select(profile => profile.Id).ToList();

            await _userProfileRepository.AddManyAsync(profiles, cancellationToken);
            await Context.PersonalChats.AddAsync(chat, cancellationToken);

            return chat;
        }
    }
}

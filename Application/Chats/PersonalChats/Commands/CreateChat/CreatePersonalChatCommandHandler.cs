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
        private readonly IRelationshipRepository _relationshipRepository;
        public CreatePersonalChatCommandHandler(IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IUserProfileRepository userProfileRepository,
            IRoleFactory roleFactory,
            IRelationshipRepository relationshipRepository)
            : base(context, userProvider)
        {
            _userProfileRepository = userProfileRepository;
            _roleFactory = roleFactory;
            _relationshipRepository = relationshipRepository;
        }

        public async Task<PersonalChat> Handle(CreatePersonalChatCommand command, CancellationToken cancellationToken)
        {
            Relationship? relationship = await _relationshipRepository
                .FindOrDefaultAsync((command.UserId, UserId), cancellationToken);

            if (relationship != null && relationship.PersonalChatId != null)
                throw new InvalidOperationException("Chat already exists");

            if (relationship == RelationshipTypes.Blocked)
            {
                string exceptionMessage = relationship.Active == UserId ?
                    "You block this user" :
                    "You blocked by this user";

                throw new InvalidOperationException(exceptionMessage);
            }

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

            if (relationship is not null)
            {
                relationship.PersonalChatId = chat.Id;
                await _relationshipRepository.UpdateAsync(relationship, cancellationToken);
            }
            else
            {
                relationship = new()
                {
                    Active = UserId,
                    Passive = command.UserId,
                    RelationshipType = RelationshipTypes.Acquaintance,
                    PersonalChatId = chat.Id
                };

                await _relationshipRepository.AddAsync(relationship, cancellationToken);
            }

            chat.Profiles = profiles.Select(profile => profile.Id).ToList();

            await _userProfileRepository.AddManyAsync(profiles, cancellationToken);
            await Context.PersonalChats.AddAsync(chat, cancellationToken);

            return chat;
        }
    }
}

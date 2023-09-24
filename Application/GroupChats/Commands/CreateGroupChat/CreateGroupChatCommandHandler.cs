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
        public async Task<string> Handle(CreateGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = new()
            {
                Title = command.Title,
                Image = command.Image,
            };

            //TODO Добавить роли новым пользователям
            List<UserProfile> profiles = command.UsersId.ConvertAll(id => new UserProfile()
            {
                UserId = id,
                ChatId = chat.Id
            });

            UserProfile ownerProfile = new()
            {
                UserId = UserId,
                ChatId = chat.Id
            };

            profiles.Add(ownerProfile);
            chat.Profiles = profiles.ConvertAll(p => p.Id);
            chat.OwnerId = ownerProfile.Id;

            await _userProfileRepository.AddManyAsync(profiles);
            await Context.GroupChats.AddAsync(chat);
            return chat.Id;
        }

        public CreateGroupChatCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper, IUserProfileRepository userProfileRepository) : base(context, userProvider, mapper)
        {
            _userProfileRepository = userProfileRepository;
        }
    }
}
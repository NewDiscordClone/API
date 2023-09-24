using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.AddMemberToGroupChat
{
    public class AddMemberToGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<AddMemberToGroupChatCommand>
    {
        private readonly IUserProfileRepository _repository;
        public async Task Handle(AddMemberToGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(command.ChatId);

            if (await _repository.ChatContainsUserAsync(chat.Id, command.NewMemberId))
                throw new NoPermissionsException("User is already a member of the chat");

            //TODO Добавить роли новому пользователю
            UserProfile profile = new()
            {
                ChatId = chat.Id,
                UserId = command.NewMemberId
            };

            chat.Profiles.Add(profile.Id);
            await _repository.AddAsync(profile);
            await Context.GroupChats.UpdateAsync(chat);
        }

        public AddMemberToGroupChatCommandHandler(IAuthorizedUserProvider userProvider, IUserProfileRepository repository) : base(userProvider)
        {
            _repository = repository;
        }
    }
}
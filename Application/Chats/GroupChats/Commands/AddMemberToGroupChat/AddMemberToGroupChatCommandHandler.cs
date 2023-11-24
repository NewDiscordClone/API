using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.GroupChats.Commands.AddMemberToGroupChat
{
    public class AddMemberToGroupChatCommandHandler : IRequestHandler<AddMemberToGroupChatCommand, GroupChat>
    {
        private readonly IUserProfileRepository _repository;
        private readonly IRoleFactory _roleFactory;
        private readonly IChatRepository _chatRepository;
        public async Task<GroupChat> Handle(AddMemberToGroupChatCommand command, CancellationToken cancellationToken)
        {
            GroupChat chat = await _chatRepository.FindAsync<GroupChat>(command.ChatId, cancellationToken);

            if (await _repository.ChatContainsUserAsync(chat.Id, command.NewMemberId, cancellationToken))
                throw new NoPermissionsException("User is already a member of the chat");

            UserProfile profile = new()
            {
                ChatId = chat.Id,
                UserId = command.NewMemberId,
                Roles = { _roleFactory.GroupChatMemberRole }
            };

            chat.Profiles.Add(profile.Id);
            await _repository.AddAsync(profile, cancellationToken);
            await _chatRepository.UpdateAsync(chat, cancellationToken);

            return chat;
        }

        public AddMemberToGroupChatCommandHandler(IUserProfileRepository repository,
            IRoleFactory roleFactory,
            IChatRepository chatRepository)
        {
            _repository = repository;
            _roleFactory = roleFactory;
            _chatRepository = chatRepository;
        }
    }
}
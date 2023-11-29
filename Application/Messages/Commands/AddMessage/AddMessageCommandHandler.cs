using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.AddMessage
{
    public class AddMessageCommandHandler : RequestHandler, IRequestHandler<AddMessageCommand, Message>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;

        public async Task<Message> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            Chat chat = await _chatRepository.FindAsync(request.ChatId, cancellationToken);
            UserProfile? profile = null;

            if (chat is Channel channel)
            {
                profile = await _serverProfileRepository.
                    FindUserProfileOnServerAsync(channel.ServerId, UserId, cancellationToken)
                    ?? throw new EntityNotFoundException(message: $"User {UserId} profile not found" +
                        $" in server {channel.ServerId}", "");
            }
            else if (chat is PersonalChat personalChat && chat is not GroupChat)
            {
                Relationship relationship = await _relationshipRepository
                    .FindByChatIdAsync(chat.Id, cancellationToken);

                if (relationship == RelationshipTypes.Blocked)
                {
                    string exceptionMessage = relationship.Active == UserId ?
                    "You block this user" :
                    "You blocked by this user";

                    throw new InvalidOperationException(exceptionMessage);
                }
            }

            profile ??= await _userProfileRepository
                .FindByChatIdAndUserIdAsync(chat.Id, UserId, cancellationToken);

            List<Attachment> attachments = [];

            request.Text.GetAttachments(attachments.Add);

            request.Attachments?.ForEach(a =>
            {
                attachments.Add(new Attachment
                {
                    IsInText = false,
                    Path = a.Path,
                    IsSpoiler = a.IsSpoiler
                });
            });

            Message message = new()
            {
                Text = request.Text,
                ChatId = request.ChatId,
                SendTime = DateTime.UtcNow,
                AuthorProfile = profile.Id,
                Author = profile.UserId,
                Attachments = attachments
            };

            await _messageRepository.AddAsync(message, cancellationToken);

            chat.UpdatedDate = message.SendTime;
            await _chatRepository.UpdateAsync(chat, cancellationToken);

            return message;
        }

        public AddMessageCommandHandler(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IUserProfileRepository userProfileRepository,
            IServerProfileRepository serverProfileRepository,
            IRelationshipRepository relationshipRepository,
            IMessageRepository messageRepository,
            IChatRepository chatRepository,
            IUserRepository userRepository) : base(userProvider, mapper)
        {
            _userProfileRepository = userProfileRepository;
            _serverProfileRepository = serverProfileRepository;
            _relationshipRepository = relationshipRepository;
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }
    }
}
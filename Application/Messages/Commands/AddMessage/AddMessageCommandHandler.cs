using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Messages.Commands.AddMessage
{
    public class AddMessageCommandHandler : RequestHandlerBase, IRequestHandler<AddMessageCommand, MessageDto>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IRelationshipRepository _relationshipRepository;

        public async Task<MessageDto> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Chat chat = await Context.Chats.FindAsync(request.ChatId, cancellationToken);
            UserProfile? profile = null;
            if (chat is Channel channel)
            {
                profile = await _serverProfileRepository.
                    FindUserProfileOnServerAsync(channel.ServerId, UserId, cancellationToken)
                    ?? throw new EntityNotFoundException(message: $"User {UserId} profile not found" +
                    $" in server {channel.ServerId}", "");
            }
            else if (chat is PersonalChat personalChat)
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

            List<Attachment> attachments = new();

            AttachmentsFromText.GetAttachments(request.Text, a => attachments.Add(a));

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

            await Context.Messages.AddAsync(message, cancellationToken);

            chat.UpdatedDate = message.SendTime;
            await Context.Chats.UpdateAsync(chat, cancellationToken);

            MessageDto dto = Mapper.Map<MessageDto>(message);

            User? user = await Context.Users.FindAsync(new object?[] { UserId }, cancellationToken: cancellationToken)!;

            dto.Author = Mapper.Map<UserViewModel>(user);

            if (profile is not null and ServerProfile serverProfile)
            {
                dto.Author.DisplayName = serverProfile.DisplayName
                    ?? dto.Author.DisplayName;
            }

            return dto;
        }

        public AddMessageCommandHandler(IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IUserProfileRepository userProfileRepository,
            IServerProfileRepository serverProfileRepository,
            IRelationshipRepository relationshipRepository) : base(context, userProvider, mapper)
        {
            _userProfileRepository = userProfileRepository;
            _serverProfileRepository = serverProfileRepository;
            _relationshipRepository = relationshipRepository;
        }
    }
}
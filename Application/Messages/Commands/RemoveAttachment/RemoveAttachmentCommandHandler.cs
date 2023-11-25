using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveAttachment
{
    public class RemoveAttachmentCommandHandler : RequestHandler, IRequestHandler<RemoveAttachmentCommand, Message>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMessageRepository _messageRepository;

        public async Task<Message> Handle(RemoveAttachmentCommand command, CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.FindAsync(command.MessageId, cancellationToken);

            if (message.Author != UserId)
                throw new NoPermissionsException("This isn't your reaction");

            string path;

            try
            {
                path = message.Attachments[command.AttachmentIndex].Path;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidOperationException($"Attachment by {command.AttachmentIndex} index does not exists");
            }

            message.Attachments.RemoveAt(command.AttachmentIndex);

            await _messageRepository.UpdateAsync(message, cancellationToken);

            //TODO Remove media
            //await Context.CheckRemoveMedia(path[(path.LastIndexOf('/') - 1)..]);

            return message;
        }

        public RemoveAttachmentCommandHandler(IAuthorizedUserProvider userProvider,
            IUserProfileRepository userProfileRepository,
            IMessageRepository messageRepository) : base(userProvider)
        {
            _userProfileRepository = userProfileRepository;
            _messageRepository = messageRepository;
        }
    }
}
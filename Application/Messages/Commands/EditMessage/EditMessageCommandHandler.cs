using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.EditMessage
{
    public class EditMessageCommandHandler(IAuthorizedUserProvider userProvider, IMessageRepository messageRepository)
        : RequestHandlerBase(userProvider), IRequestHandler<EditMessageCommand, Message>
    {
        private readonly IMessageRepository _messageRepository = messageRepository;

        public async Task<Message> Handle(EditMessageCommand command, CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.FindAsync(command.MessageId, cancellationToken);

            if (message.Author != UserId)
                throw new NoPermissionsException("You don't have permission to edit the message");

            message.Text = command.NewText;
            message.Attachments.RemoveAll(a => a.IsInText);

            List<Attachment> attachments = [];
            command.NewText.GetAttachments(attachments.Add);
            attachments.AddRange(message.Attachments);
            message.Attachments = attachments;

            await _messageRepository.UpdateAsync(message, cancellationToken);

            return message;
        }
    }
}
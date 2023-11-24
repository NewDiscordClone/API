using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.PinMessage
{
    public class PinMessageCommandHandler(IAuthorizedUserProvider userProvider, IMessageRepository messageRepository)
        : RequestHandlerBase(userProvider), IRequestHandler<PinMessageCommand, Message>
    {
        private readonly IMessageRepository _messageRepository = messageRepository;

        public async Task<Message> Handle(PinMessageCommand command, CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.FindAsync(command.MessageId);

            message.IsPinned = true;
            message.PinnedTime = DateTime.Now;

            await _messageRepository.UpdateAsync(message, cancellationToken);
            return message;
        }
    }
}
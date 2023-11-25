using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.UnpinMessage
{
    public class UnpinMessageCommandHandler(IAuthorizedUserProvider userProvider,
        IMessageRepository messageRepository) : RequestHandler(userProvider), IRequestHandler<UnpinMessageCommand, Message>
    {

        private readonly IMessageRepository _messageRepository = messageRepository;

        public async Task<Message> Handle(UnpinMessageCommand request, CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.FindAsync(request.MessageId, cancellationToken);

            message.IsPinned = false;
            await _messageRepository.UpdateAsync(message, cancellationToken);

            return message;
        }
    }
}
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveAllReactions
{
    public class RemoveAllReactionsCommandHandler(IAuthorizedUserProvider userProvider,
        IMessageRepository messageRepository)
        : RequestHandlerBase(userProvider), IRequestHandler<RemoveAllReactionsCommand, Message>
    {

        private readonly IMessageRepository _messageRepository = messageRepository;
        private readonly IChatRepository _chatRepository;

        public async Task<Message> Handle(RemoveAllReactionsCommand command, CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.FindAsync(command.MessageId, cancellationToken);
            Chat chat = await _chatRepository.FindAsync(message.ChatId, cancellationToken);

            message.Reactions = [];

            await _messageRepository.UpdateAsync(message, cancellationToken);
            return message;
        }
    }
}
using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.AddReaction
{
    public class AddReactionCommandHandler(IAuthorizedUserProvider userProvider,
        IMapper mapper,
        IMessageRepository messageRepository)
        : RequestHandler(userProvider, mapper), IRequestHandler<AddReactionCommand, Message>
    {
        private readonly IMessageRepository _messageRepository = messageRepository;
        public async Task<Message> Handle(AddReactionCommand command, CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.FindAsync(command.MessageId, cancellationToken);

            Reaction reaction = new()
            {
                AuthorProfile = message.AuthorProfile,
                Emoji = command.Emoji,
            };

            if (message.Reactions.Contains(reaction))
                throw new InvalidOperationException("Reaction already exists");

            message.Reactions.Add(reaction);

            await _messageRepository.UpdateAsync(message, cancellationToken);

            return message;
        }
    }
}
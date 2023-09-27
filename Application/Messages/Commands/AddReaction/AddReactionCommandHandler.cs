using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.AddReaction
{
    public class AddReactionCommandHandler : RequestHandlerBase, IRequestHandler<AddReactionCommand>
    {
        public async Task Handle(AddReactionCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId, cancellationToken);

            Reaction reaction = new()
            {
                AuthorProfile = message.Author,
                Emoji = command.Emoji,
            };

            if (message.Reactions.Contains(reaction))
                throw new InvalidOperationException("Reaction already exists");

            message.Reactions.Add(reaction);

            await Context.Messages.UpdateAsync(message, cancellationToken);
        }

        public AddReactionCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}
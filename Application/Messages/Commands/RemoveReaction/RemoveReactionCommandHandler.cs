using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveReaction
{
    public class RemoveReactionCommandHandler : RequestHandlerBase, IRequestHandler<RemoveReactionCommand>
    {
        public async Task Handle(RemoveReactionCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId);
            Reaction reaction = message.Reactions[command.ReactionIndex];

            if (reaction.User != UserId)
                throw new NoPermissionsException("This isn't your reaction");

            message.Reactions.RemoveAt(command.ReactionIndex);
            await Context.Messages.UpdateAsync(message);
        }

        public RemoveReactionCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
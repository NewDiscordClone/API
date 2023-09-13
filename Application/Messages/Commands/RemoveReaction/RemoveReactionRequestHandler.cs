using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveReaction
{
    public class RemoveReactionRequestHandler : RequestHandlerBase, IRequestHandler<RemoveReactionRequest>
    {
        public async Task Handle(RemoveReactionRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(request.MessageId);
            Reaction reaction = message.Reactions[request.ReactionIndex];

            if (reaction.User != UserId)
                throw new NoPermissionsException("This isn't your reaction");

            message.Reactions.RemoveAt(request.ReactionIndex);
            await Context.Messages.UpdateAsync(message);
        }

        public RemoveReactionRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Messages.RemoveReaction
{

    public class RemoveReactionRequestHandler : RequestHandlerBase, IRequestHandler<RemoveReactionRequest>
    {
        public async Task Handle(RemoveReactionRequest request, CancellationToken cancellationToken)
        {
            Reaction? reaction = await Context.FindByIdAsync<Reaction>(request.ReactionId, cancellationToken);
            if (reaction.User.Id != UserId)
                throw new NoPermissionsException("This isn't your reaction");

            Context.Reactions.Remove(reaction);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public RemoveReactionRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}

using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Messages.RemoveReaction
{
    public class RemoveReactionRequestHandler : RequestHandlerBase, IRequestHandler<RemoveReactionRequest, Message>
    {
        public async Task<Message> Handle(RemoveReactionRequest request, CancellationToken cancellationToken)
        {
            Reaction? reaction = await Context.FindByIdAsync<Reaction>(request.ReactionId, cancellationToken,
                "Message",
                "Message.Chat",
                "Message.Chat.Users"
            );
            if (reaction.User.Id != UserId)
                throw new NoPermissionsException("This isn't your reaction");
            Message message = reaction.Message;

            Context.Reactions.Remove(reaction);
            await Context.SaveChangesAsync(cancellationToken);
            return message;
        }

        public RemoveReactionRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
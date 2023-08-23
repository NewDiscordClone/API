using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.Messages.RemoveReaction
{
    public class RemoveReactionRequestHandler : RequestHandlerBase, IRequestHandler<RemoveReactionRequest, Message>
    {
        public async Task<Message> Handle(RemoveReactionRequest request, CancellationToken cancellationToken)
        {
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken);
            Reaction reaction = message.Reactions[request.ReactionIndex];

            if (reaction.User.Id != UserId)
                throw new NoPermissionsException("This isn't your reaction");

            await Context.Messages.UpdateOneAsync(Context.GetIdFilter<Message>(message.Id),
                Builders<Message>.Update.Pull(m => m.Reactions, reaction),
                null,
                cancellationToken);
            return message;
        }

        public RemoveReactionRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
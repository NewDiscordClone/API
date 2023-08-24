using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Messages.AddReaction
{

    public class AddReactionRequestHandler : RequestHandlerBase, IRequestHandler<AddReactionRequest, Reaction>
    {
        public async Task<Reaction> Handle(AddReactionRequest request, CancellationToken cancellationToken)
        {
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken, 
                "Chat",
                "Chat.Users"
                );
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            if (!message.Chat.Users.Contains(user))
                throw new NoPermissionsException("You are not a member of the Chat");
            
            Reaction reaction = new()
            {
                User = user,
                Message = message,
                Emoji = request.Emoji,
            };
            await Context.Reactions.AddAsync(reaction, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return reaction;
        }

        public AddReactionRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}

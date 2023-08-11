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
            Message? message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken);
            User? user = await Context.FindByIdAsync<User>(UserId, cancellationToken);
            
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

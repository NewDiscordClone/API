using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Messages.RemoveAllReactions
{

    public class RemoveAllReactionsRequestHandler : RequestHandlerBase, IRequestHandler<RemoveAllReactionsRequest, Chat>
    {
        public async Task<Chat> Handle(RemoveAllReactionsRequest request, CancellationToken cancellationToken)
        {
            Message? message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken,
                "Reactions",
                "Chat",
                "Chat.Users");

            Context.Reactions.RemoveRange(message.Reactions);
            await Context.SaveChangesAsync(cancellationToken);
            return message.Chat;
        }

        public RemoveAllReactionsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}

using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveAllReactions
{
    public class RemoveAllReactionsRequestHandler : RequestHandlerBase, IRequestHandler<RemoveAllReactionsRequest>
    {
        public async Task Handle(RemoveAllReactionsRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(request.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);

            message.Reactions = new List<Reaction>();

            await Context.Messages.UpdateAsync(message);
        }

        public RemoveAllReactionsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
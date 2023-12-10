using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;
using Sparkle.Domain.Messages;
using Sparkle.Domain.Messages.ValueObjects;

namespace Sparkle.Application.Messages.Commands.RemoveAllReactions
{
    public class RemoveAllReactionsCommandHandler : RequestHandlerBase, IRequestHandler<RemoveAllReactionsCommand>
    {
        public async Task Handle(RemoveAllReactionsCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);

            message.Reactions = new List<Reaction>();

            await Context.Messages.UpdateAsync(message);
        }

        public RemoveAllReactionsCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
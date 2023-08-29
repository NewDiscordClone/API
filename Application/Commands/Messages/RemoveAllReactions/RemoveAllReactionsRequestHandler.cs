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
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(request.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);

            message.Reactions = new List<Reaction>();

            await Context.Messages.UpdateAsync(message);
            return chat;
        }

        public RemoveAllReactionsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
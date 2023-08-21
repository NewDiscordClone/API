using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Messages.UnpinMessage
{

    public class UnpinMessageRequestHandler : RequestHandlerBase, IRequestHandler<UnpinMessageRequest, Message>
    {
        public async Task<Message> Handle(UnpinMessageRequest request, CancellationToken cancellationToken)
        {
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken,
                "Chat",
                "Chat.Users",
                "Chat.Messages");
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            if (!message.Chat.Users.Contains(user))
                throw new NoPermissionsException("You are not a member of the Chat");

            message.IsPinned = false;
            await Context.SaveChangesAsync(cancellationToken);

            return message;
        }

        public UnpinMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}

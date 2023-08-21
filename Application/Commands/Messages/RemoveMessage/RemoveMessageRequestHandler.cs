using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Messages.RemoveMessage
{

    public class RemoveMessageRequestHandler : RequestHandlerBase, IRequestHandler<RemoveMessageRequest, Chat>
    {
        public async Task<Chat> Handle(RemoveMessageRequest request, CancellationToken cancellationToken)
        {
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken,
                "User",
                "Chat",
                "Chat.Users");

            bool isOwner = false;

            if (message.Chat is Channel channel)
                isOwner = UserProvider.IsInRole("Owner", channel.Server.Id);

            if (message.User.Id != UserId || isOwner)
                throw new NoPermissionsException("You don't have permission to remove the message");


            Chat chat = message.Chat;

            Context.Messages.Remove(message);
            await Context.SaveChangesAsync(cancellationToken);

            return chat;
        }

        public RemoveMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}

using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

            if (message.User.Id != UserId)
            {
                Channel? channel = await Context.Channels
                    .Include(c => c.Server)
                    .Include(c => c.Server.Owner)
                    .FirstOrDefaultAsync(c => c.Id == message.Chat.Id,
                        cancellationToken: cancellationToken);
                if (channel == null || channel.Server.Owner.Id != UserId) 
                    throw new NoPermissionsException("You don't have permission to remove the message");
            }

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

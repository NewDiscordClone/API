using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            
            Channel? channel = await Context.Channels
                .Include(c => c.Server)
                .Include(c => c.Server.Owner)
                .FirstOrDefaultAsync(c => c.Id == message.Chat.Id,
                    cancellationToken: cancellationToken);
            if (channel != null && channel.Server.Owner.Id != user.Id) 
                throw new NoPermissionsException("You are not the Owner of the Server");

            message.IsPinned = false;
            await Context.SaveChangesAsync(cancellationToken);

            return message;
        }

        public UnpinMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}

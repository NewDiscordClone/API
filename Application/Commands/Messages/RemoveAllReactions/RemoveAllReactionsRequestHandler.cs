using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            
            Channel? channel = await Context.Channels
                .Include(c => c.Server)
                .Include(c => c.Server.Owner)
                .FirstOrDefaultAsync(c => c.Id == message.Chat.Id,
                    cancellationToken: cancellationToken);
            if (channel != null && channel.Server.Owner.Id != UserId) 
                throw new NoPermissionsException("You don't have permission to remove the message reactions");

            Context.Reactions.RemoveRange(message.Reactions);
            await Context.SaveChangesAsync(cancellationToken);
            return message.Chat;
        }

        public RemoveAllReactionsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}

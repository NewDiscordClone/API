using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Commands.Messages.RemoveAllReactions
{
    public class RemoveAllReactionsRequestHandler : RequestHandlerBase, IRequestHandler<RemoveAllReactionsRequest, Chat>
    {
        public async Task<Chat> Handle(RemoveAllReactionsRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            
            Message message = await Context.Messages.FindAsync(request.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);

            //TODO: Перевірка на наявність відповідної ролі
            // Channel? channel = await Context.Channels
            //     .Include(c => c.Server)
            //     .Include(c => c.Server.Owner)
            //     .FirstOrDefaultAsync(c => c.Id == message.Chat.Id,
            //         cancellationToken: cancellationToken);
            // if (channel != null && channel.Server.Owner.Id != UserId) 
            //     throw new NoPermissionsException("You don't have permission to remove the message reactions");

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
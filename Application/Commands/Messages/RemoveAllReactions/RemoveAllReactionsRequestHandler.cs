using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Commands.Messages.RemoveAllReactions
{
    public class RemoveAllReactionsRequestHandler : RequestHandlerBase, IRequestHandler<RemoveAllReactionsRequest, Chat>
    {
        public async Task<Chat> Handle(RemoveAllReactionsRequest request, CancellationToken cancellationToken)
        {
            Message? message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken);
            Chat? chat = await Context.FindByIdAsync<Chat>(message.ChatId, cancellationToken);

            //TODO: Перевірка на наявність відповідної ролі
            // Channel? channel = await Context.Channels
            //     .Include(c => c.Server)
            //     .Include(c => c.Server.Owner)
            //     .FirstOrDefaultAsync(c => c.Id == message.Chat.Id,
            //         cancellationToken: cancellationToken);
            // if (channel != null && channel.Server.Owner.Id != UserId) 
            //     throw new NoPermissionsException("You don't have permission to remove the message reactions");

            await Context.Messages.UpdateOneAsync(
                Context.GetIdFilter<Message>(message.Id),
                Builders<Message>.Update.Set(m => m.Reactions, new List<Reaction>()),
                null,
                cancellationToken
            );
            return chat;
        }

        public RemoveAllReactionsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
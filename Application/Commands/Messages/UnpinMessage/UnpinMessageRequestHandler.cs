using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Commands.Messages.UnpinMessage
{

    public class UnpinMessageRequestHandler : RequestHandlerBase, IRequestHandler<UnpinMessageRequest, Message>
    {
        public async Task<Message> Handle(UnpinMessageRequest request, CancellationToken cancellationToken)
        {
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken);
            Chat chat = await Context.FindByIdAsync<Chat>(message.ChatId, cancellationToken);

            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            //TODO: Перевірка на відповідну роль на сервері
            
            await Context.Messages.UpdateOneAsync(
                Context.GetIdFilter<Message>(request.MessageId),
                Builders<Message>.Update.Set(m => m.IsPinned, false),
                null,
                cancellationToken
            );
            
            return await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken);
        }

        public UnpinMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}

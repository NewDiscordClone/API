using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Commands.Messages.RemoveAttachment
{
    public class RemoveAttachmentRequestHandler : RequestHandlerBase, IRequestHandler<RemoveAttachmentRequest, Chat>
    {
        public async Task<Chat> Handle(RemoveAttachmentRequest request, CancellationToken cancellationToken)
        {
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken);
            Chat chat = await Context.FindByIdAsync<Chat>(message.ChatId, cancellationToken);
            User user = await Context.FindSqlByIdAsync<User>(UserId, cancellationToken);

            if (message.User.Id != user.Id)
                throw new NoPermissionsException("You don't have permission to edit the message");

            await Context.Messages.UpdateOneAsync(
                Context.GetIdFilter<Message>(message.Id),
                Builders<Message>.Update.Pull(m => m.Attachments,
                    message.Attachments[request.AttachmentIndex]),
                null,
                cancellationToken
            );
            return chat;
        }

        public RemoveAttachmentRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
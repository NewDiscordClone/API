using System.Text.RegularExpressions;
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.Messages.EditMessage
{
    public class EditMessageRequestHandler : RequestHandlerBase, IRequestHandler<EditMessageRequest, Message>
    {
        public async Task<Message> Handle(EditMessageRequest request, CancellationToken cancellationToken)
        {
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken);
            
            if (message.User.Id != UserId)
                throw new NoPermissionsException("You don't have permission to edit the message");
            

            var filter = Context.GetIdFilter<Message>(request.MessageId);
            
            await Context.Messages.UpdateOneAsync(
                filter,
                Builders<Message>.Update
                    .PullFilter(m => m.Attachments, a =>
                        a.Type == AttachmentType.Url ||
                        a.Type == AttachmentType.UrlGif ||
                        a.Type == AttachmentType.UrlImage)
                    .Set(m => m.Text, request.NewText),
                null,
                cancellationToken
            );
            
            message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken);
            List<Attachment> attachments = message.Attachments;

            AttachmentsFromText.GetAttachments(request.NewText, a => attachments.Add(a));
            
            await Context.Messages.UpdateOneAsync(
                filter,
                Builders<Message>.Update
                    .Set(m => m.Attachments, attachments),
                null,
                cancellationToken
            );
            return message;
        }

        public EditMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
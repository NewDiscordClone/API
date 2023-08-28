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

            message.Attachments.RemoveAll(a => a.IsInText);
            
            List<Attachment> attachments = new List<Attachment>();
            AttachmentsFromText.GetAttachments(request.NewText, a => attachments.Add(a));
            attachments.AddRange(message.Attachments);
            
            await Context.Messages.UpdateOneAsync(
                filter,
                Builders<Message>.Update
                    .Set(m => m.Attachments, attachments)
                    .Set(m => m.Text, request.NewText),
                null,
                cancellationToken
            );
            return await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken);
        }

        public EditMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
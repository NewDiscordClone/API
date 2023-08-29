using System.Text.RegularExpressions;
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.Messages.EditMessage
{
    public class EditMessageRequestHandler : RequestHandlerBase, IRequestHandler<EditMessageRequest, Message>
    {
        public async Task<Message> Handle(EditMessageRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            
            Message message = await Context.Messages.FindAsync(request.MessageId);

            if (message.User.Id != UserId)
                throw new NoPermissionsException("You don't have permission to edit the message");

            message.Text = request.NewText;
            message.Attachments.RemoveAll(a => a.IsInText);

            List<Attachment> attachments = new List<Attachment>();
            AttachmentsFromText.GetAttachments(request.NewText, a => attachments.Add(a));
            attachments.AddRange(message.Attachments);
            message.Attachments = attachments;

            return await Context.Messages.UpdateAsync(message);
        }

        public EditMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
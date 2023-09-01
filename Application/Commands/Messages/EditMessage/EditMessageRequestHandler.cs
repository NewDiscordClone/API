using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Messages.EditMessage
{
    public class EditMessageRequestHandler : RequestHandlerBase, IRequestHandler<EditMessageRequest>
    {
        public async Task Handle(EditMessageRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(request.MessageId);

            if (message.User.Id != UserId)
                throw new NoPermissionsException("You don't have permission to edit the message");

            message.Text = request.NewText;
            message.Attachments.RemoveAll(a => a.IsInText);

            List<Attachment> attachments = new();
            AttachmentsFromText.GetAttachments(request.NewText, a => attachments.Add(a));
            attachments.AddRange(message.Attachments);
            message.Attachments = attachments;

            await Context.Messages.UpdateAsync(message);
        }

        public EditMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
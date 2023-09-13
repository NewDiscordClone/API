using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.EditMessage
{
    public class EditMessageRequestHandler : RequestHandlerBase, IRequestHandler<EditMessageRequest>
    {
        public async Task Handle(EditMessageRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(request.MessageId);

            if (message.User != UserId)
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
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain.Messages;
using Sparkle.Domain.Messages.ValueObjects;

namespace Sparkle.Application.Messages.Commands.EditMessage
{
    public class EditMessageCommandHandler : RequestHandlerBase, IRequestHandler<EditMessageCommand>
    {
        public async Task Handle(EditMessageCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId, cancellationToken);

            if (message.Author != UserId)
                throw new NoPermissionsException("You don't have permission to edit the message");

            message.Text = command.NewText;
            message.Attachments.RemoveAll(a => a.IsInText);

            List<Attachment> attachments = new();
            AttachmentsFromText.GetAttachments(command.NewText, a => attachments.Add(a));
            attachments.AddRange(message.Attachments);
            message.Attachments = attachments;

            await Context.Messages.UpdateAsync(message, cancellationToken);
        }

        public EditMessageCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider)
            : base(context, userProvider)
        {
        }
    }
}
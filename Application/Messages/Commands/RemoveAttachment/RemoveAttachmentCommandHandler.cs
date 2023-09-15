using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveAttachment
{
    public class RemoveAttachmentCommandHandler : RequestHandlerBase, IRequestHandler<RemoveAttachmentCommand>
    {
        public async Task Handle(RemoveAttachmentCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId);
            User user = await Context.SqlUsers.FindAsync(UserId);

            if (message.User != user.Id)
                throw new NoPermissionsException("You don't have permission to edit the message");

            string path = message.Attachments[command.AttachmentIndex].Path;

            message.Attachments.RemoveAt(command.AttachmentIndex);

            await Context.Messages.UpdateAsync(message);
            await Context.CheckRemoveMedia(path[(path.LastIndexOf('/') - 1)..]);
        }

        public RemoveAttachmentCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
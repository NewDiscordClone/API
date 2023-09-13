using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveAttachment
{
    public class RemoveAttachmentRequestHandler : RequestHandlerBase, IRequestHandler<RemoveAttachmentRequest>
    {
        public async Task Handle(RemoveAttachmentRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(request.MessageId);
            User user = await Context.SqlUsers.FindAsync(UserId);

            if (message.User != user.Id)
                throw new NoPermissionsException("You don't have permission to edit the message");

            string path = message.Attachments[request.AttachmentIndex].Path;

            message.Attachments.RemoveAt(request.AttachmentIndex);

            await Context.Messages.UpdateAsync(message);
            await Context.CheckRemoveMedia(path[(path.LastIndexOf('/') - 1)..]);
        }

        public RemoveAttachmentRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
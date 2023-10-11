using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveAttachment
{
    public class RemoveAttachmentCommandHandler : RequestHandlerBase, IRequestHandler<RemoveAttachmentCommand>
    {
        private readonly Common.Interfaces.Repositories.IUserProfileRepository _userProfileRepository;
        public async Task Handle(RemoveAttachmentCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId, cancellationToken);

            if (message.Author != UserId)
                throw new NoPermissionsException("This isn't your reaction");

            string path;

            try
            {
                path = message.Attachments[command.AttachmentIndex].Path;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidOperationException($"Attachment by {command.AttachmentIndex} index does not exists");
            }
            message.Attachments.RemoveAt(command.AttachmentIndex);

            await Context.Messages.UpdateAsync(message, cancellationToken);
            await Context.CheckRemoveMedia(path[(path.LastIndexOf('/') - 1)..]);
        }

        public RemoveAttachmentCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, Common.Interfaces.Repositories.IUserProfileRepository userProfileRepository) : base(context,
            userProvider)
        {
            _userProfileRepository = userProfileRepository;
        }
    }
}
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Messages.RemoveAttachment
{
    public class RemoveAttachmentRequestHandler : RequestHandlerBase, IRequestHandler<RemoveAttachmentRequest, Chat>
    {
        public async Task<Chat> Handle(RemoveAttachmentRequest request, CancellationToken cancellationToken)
        {
            Attachment attachment = await Context.FindByIdAsync<Attachment>(request.AttachmentId, cancellationToken,
                "Message",
                "Message.User",
                "Message.Chat",
                "Message.Chat.Users"
                );
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            if (attachment.Message == null || attachment.Message.User.Id != user.Id)
                throw new NoPermissionsException("You don't have permission to edit the message");
            Chat chat = attachment.Message.Chat;

            Context.Attachments.Remove(attachment);
            await Context.SaveChangesAsync(cancellationToken);
            return chat;
        }

        public RemoveAttachmentRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
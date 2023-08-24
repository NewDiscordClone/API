using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Messages.EditMessage
{
    public class EditMessageRequestHandler : RequestHandlerBase, IRequestHandler<EditMessageRequest, Message>
    {
        public async Task<Message> Handle(EditMessageRequest request, CancellationToken cancellationToken)
        {
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken,
                "User",
                "Chat",
                "Chat.Users",
                "Attachments");
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            if (message.User.Id != user.Id)
                throw new NoPermissionsException("You don't have permission to edit the message");

            message.Text = request.NewText;
            Context.Attachments.RemoveRange(message.Attachments.Where(a =>
                a.Type is
                    AttachmentType.Url or
                    AttachmentType.UrlGif or
                    AttachmentType.UrlImage
            ));
            await Context.SaveChangesAsync(cancellationToken);
            List<Attachment> attachments = await Context.Attachments
                .Include(a => a.Message)
                .Where(a => a.Message != null && a.Message.Id == message.Id).ToListAsync(cancellationToken);

            AttachmentsFromText.GetAttachments(request.NewText, a => attachments.Add(a));
            message.Attachments = attachments;
            await Context.SaveChangesAsync(cancellationToken);
            return message;
        }

        public EditMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
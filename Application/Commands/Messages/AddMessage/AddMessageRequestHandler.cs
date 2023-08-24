using System.Text.RegularExpressions;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Messages.AddMessage
{
    public class AddMessageRequestHandler : RequestHandlerBase, IRequestHandler<AddMessageRequest, Message>
    {
        
        public async Task<Message> Handle(AddMessageRequest request, CancellationToken cancellationToken)
        {
            Chat chat = await Context.FindByIdAsync<Chat>(request.ChatId, cancellationToken, "Users");
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            if (!chat.Users.Contains(user))
                throw new NoPermissionsException("You are not a member of the Chat");
            List<Attachment> attachments = new List<Attachment>();
            
            AttachmentsFromText.GetAttachments(request.Text, a=> attachments.Add(a));

            request.Attachments?.ForEach(a =>
            {
                attachments.Add(new Attachment
                {
                    Path = a.Path,
                    Type = a.Type,
                    IsSpoiler = a.IsSpoiler
                });
            });

            Message message = new()
            {
                Text = request.Text,
                Chat = chat,
                SendTime = DateTime.UtcNow,
                User = user,
                Attachments = attachments
            };
            await Context.Messages.AddAsync(message, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return message;
        }

        public AddMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
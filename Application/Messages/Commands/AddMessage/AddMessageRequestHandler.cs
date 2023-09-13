using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.AddMessage
{
    public class AddMessageRequestHandler : RequestHandlerBase, IRequestHandler<AddMessageRequest, Message>
    {
        public async Task<Message> Handle(AddMessageRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Chat chat = await Context.Chats.FindAsync(request.ChatId);
            User user = await Context.SqlUsers.FindAsync(UserId);

            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");
            List<Attachment> attachments = new();

            AttachmentsFromText.GetAttachments(request.Text, a => attachments.Add(a));

            request.Attachments?.ForEach(a =>
            {
                attachments.Add(new Attachment
                {
                    IsInText = false,
                    Path = a.Path,
                    IsSpoiler = a.IsSpoiler
                });
            });

            Message message = new()
            {
                Text = request.Text,
                ChatId = request.ChatId,
                SendTime = DateTime.UtcNow,
                User = UserId,
                Attachments = attachments
            };
            return await Context.Messages.AddAsync(message);
        }

        public AddMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}
using System.Text.RegularExpressions;
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.Messages.AddMessage
{
    public class AddMessageRequestHandler : RequestHandlerBase, IRequestHandler<AddMessageRequest, Message>
    {
        public async Task<Message> Handle(AddMessageRequest request, CancellationToken cancellationToken)
        {
            Chat chat = await Context.FindByIdAsync<Chat>(request.ChatId, cancellationToken);
            User user = await Context.FindSqlByIdAsync<User>(UserId, cancellationToken);

            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");
            List<Attachment> attachments = new List<Attachment>();

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
                User = Mapper.Map<UserLookUp>(user),
                Attachments = attachments
            };
            await Context.Messages.InsertOneAsync(message, null, cancellationToken);
            return message;
        }

        public AddMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}
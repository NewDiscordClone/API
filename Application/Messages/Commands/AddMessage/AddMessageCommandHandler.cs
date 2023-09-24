using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Messages.Commands.AddMessage
{
    public class AddMessageCommandHandler : RequestHandlerBase, IRequestHandler<AddMessageCommand, MessageDto>
    {
        public async Task<MessageDto> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Chat chat = await Context.Chats.FindAsync(request.ChatId);
            User user = await Context.SqlUsers.FindAsync(UserId);

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
            return Mapper.Map<MessageDto>(await Context.Messages.AddAsync(message));
        }

        public AddMessageCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}
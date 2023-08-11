using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Messages.AddMessageRequest
{

    public class AddMessageRequestHandler : RequestHandlerBase, IRequestHandler<AddMessageRequest, Message>
    {
        public async Task<Message> Handle(AddMessageRequest request, CancellationToken cancellationToken)
        {
            Chat? chat = await Context.FindByIdAsync<Chat>(request.ChatId, cancellationToken, "Users");
            User? user = await Context.FindByIdAsync<User>(UserId, cancellationToken);
            
            //TODO: Перевірка на те що цей юзер в чаті

            List<Attachment> attachments = new List<Attachment>();
            request.Attachments?.ForEach(a =>
            {
                attachments.Add(new Attachment()
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

        public AddMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}

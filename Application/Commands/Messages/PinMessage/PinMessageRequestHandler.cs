using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Messages.PinMessage
{
    public class PinMessageRequestHandler : RequestHandlerBase, IRequestHandler<PinMessageRequest>
    {
        public async Task Handle(PinMessageRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(request.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);


            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            //TODO: Перевірка на відповідну роль на сервері

            message.IsPinned = true;
            message.PinnedTime = DateTime.Now;
            await Context.Messages.UpdateAsync(message);
        }

        public PinMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
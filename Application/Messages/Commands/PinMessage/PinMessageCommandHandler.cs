using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.PinMessage
{
    public class PinMessageCommandHandler : RequestHandlerBase, IRequestHandler<PinMessageCommand>
    {
        public async Task Handle(PinMessageCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);


            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            //TODO: Перевірка на відповідну роль на сервері

            message.IsPinned = true;
            message.PinnedTime = DateTime.Now;
            await Context.Messages.UpdateAsync(message);
        }

        public PinMessageCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}
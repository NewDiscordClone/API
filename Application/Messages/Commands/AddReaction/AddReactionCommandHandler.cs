using AutoMapper;
using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.AddReaction
{
    public class AddReactionCommandHandler : RequestHandlerBase, IRequestHandler<AddReactionCommand>
    {
        public async Task Handle(AddReactionCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);

            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            Reaction reaction = new()
            {
                User = UserId,
                Emoji = command.Emoji,
            };

            await Context.Messages.UpdateAsync(message);
        }

        public AddReactionCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}
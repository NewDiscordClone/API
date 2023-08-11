using Application.Exceptions;
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
            Message? message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken, 
                "Chat",
                "Chat.Users",
                "Chat.PinnedMessages");
            User? user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            if (!message.Chat.Users.Contains(user))
                throw new NoPermissionsException("You are not a member of the Chat");
            if (message.Chat.GetType() == typeof(Channel))
            {
                var channel = await Context.FindByIdAsync<Channel>(message.Chat.Id, cancellationToken, 
                    "Server",
                    "Server.Owner");
                if(channel.Server.Owner.Id != user.Id)
                    throw new NoPermissionsException("You are not the Owner of the Server");
            }
            //TODO: Перевірка на те що цей юзер в чаті

            message.IsPinned = true;
            await Context.SaveChangesAsync(cancellationToken);
        }

        public PinMessageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}

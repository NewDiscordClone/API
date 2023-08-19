using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Channels.RemoveChannel
{
    public class RemoveChannelRequestHandler : RequestHandlerBase, IRequestHandler<RemoveChannelRequest>
    {
        public async Task Handle(RemoveChannelRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            Channel chat =
                await Context.FindByIdAsync<Channel>(request.ChatId, cancellationToken, "Users");
            
            //TODO: Перевірити що у юзера є відповідні права
            if (chat.Users.Find(u => u.Id == user.Id) == null)
                throw new NoSuchUserException("User is not a member of the chat");
            Context.Chats.Remove(chat);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public RemoveChannelRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
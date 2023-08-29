using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.Channels.RemoveChannel
{
    public class RemoveChannelRequestHandler : RequestHandlerBase, IRequestHandler<RemoveChannelRequest>
    {
        public async Task Handle(RemoveChannelRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            
            Channel chat = await Context.Channels.FindAsync(request.ChatId);
            
            //TODO: Перевірити що у юзера є відповідні права
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoSuchUserException("User is not a member of the chat");
            
            await Context.Chats.DeleteAsync(chat);
        }

        public RemoveChannelRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
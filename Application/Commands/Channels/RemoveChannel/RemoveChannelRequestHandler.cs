using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.Channels.RemoveChannel
{
    public class RemoveChannelRequestHandler : RequestHandlerBase, IRequestHandler<RemoveChannelRequest>
    {
        public async Task Handle(RemoveChannelRequest request, CancellationToken cancellationToken)
        {
            Channel chat = await Context.FindByIdAsync<Channel>(request.ChatId, cancellationToken);
            
            //TODO: Перевірити що у юзера є відповідні права
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoSuchUserException("User is not a member of the chat");
            
            await Context.Chats.DeleteOneAsync(Context.GetIdFilter<Chat>(chat.Id), cancellationToken);
        }

        public RemoveChannelRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
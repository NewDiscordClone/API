using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.LeaveFromPrivateChat
{
    public class LeaveFromPrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<LeaveFromPrivateChatRequest>
    {
        public async Task Handle(LeaveFromPrivateChatRequest request, CancellationToken cancellationToken)
        {
            PrivateChat chat =
                await Context.FindByIdAsync<PrivateChat>(request.ChatId, cancellationToken);
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoSuchUserException("User is not a member of the chat");

            if (chat.Users.Count <= 2)
            {
                await Context.Chats.DeleteOneAsync(Context.GetIdFilter<Chat>(chat.Id), null, cancellationToken);
            }
            else
            {
                var removeUser = Builders<PrivateChat>.Update.PullFilter(
                    c => c.Users,
                    Builders<UserLookUp>.Filter.Eq(u => u.Id, UserId)
                );
                if (chat.OwnerId == UserId)
                {
                    removeUser = Builders<PrivateChat>.Update.Combine(
                        removeUser,
                        Builders<PrivateChat>.Update
                            .Set(c => c.OwnerId, chat.Users.First(u => u.Id != UserId).Id));
                }
                await Context.PrivateChats.UpdateOneAsync(
                    Context.GetIdFilter<PrivateChat>(chat.Id),
                    removeUser,
                    null,
                    cancellationToken);
            }
        }

        public LeaveFromPrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
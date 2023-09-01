using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.GroupChats.LeaveFromGroupChat
{
    public class LeaveFromGroupChatRequestHandler : RequestHandlerBase, IRequestHandler<LeaveFromGroupChatRequest>
    {
        public async Task Handle(LeaveFromGroupChatRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            
            GroupChat pchat = await Context.GroupChats.FindAsync(request.ChatId);
            if(pchat is not GroupChat chat) throw new Exception("This is not group chat");

            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoSuchUserException("User is not a member of the chat");

            if (chat.Users.Count <= 1)
            {
                await Context.Chats.DeleteAsync(chat);
            }
            else
            {
                chat.Users.Remove(chat.Users.Find(u => u.Id == UserId) ?? throw new NoSuchUserException());
                if (chat.OwnerId == UserId)
                    chat.OwnerId = chat.Users.First(u => u.Id != UserId).Id;

                await Context.GroupChats.UpdateAsync(chat);
            }
        }

        public LeaveFromGroupChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
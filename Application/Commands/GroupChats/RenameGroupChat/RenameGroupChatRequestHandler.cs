using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.GroupChats.RenameGroupChat
{
    public class RenameGroupChatRequestHandler : RequestHandlerBase, IRequestHandler<RenameGroupChatRequest>
    {
        public async Task Handle(RenameGroupChatRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("User is not a member of the chat");

            chat.Title = request.NewTitle;

            await Context.GroupChats.UpdateAsync(chat);
        }

        public RenameGroupChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}

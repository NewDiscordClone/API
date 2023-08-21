using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.PrivateChats.AddMemberToPrivateChat
{
    public class AddMemberToPrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<AddMemberToPrivateChatRequest>
    {
        public async Task Handle(AddMemberToPrivateChatRequest request, CancellationToken cancellationToken)
        {
            PrivateChat chat =
                await Context.FindByIdAsync<PrivateChat>(request.ChatId, cancellationToken, "Users");
            if (chat.Users.Find(u => u.Id == UserId) == null)
                throw new NoPermissionsException("User is not a member of the chat");
            if (chat.Users.Find(u => u.Id == request.NewMemberId) != null)
                throw new NoPermissionsException("User is already a member of the chat");

            User NewMember = await Context.FindByIdAsync<User>(request.NewMemberId, cancellationToken);
            chat.Users.Add(NewMember);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public AddMemberToPrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
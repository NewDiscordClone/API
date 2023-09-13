using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using MongoDB.Driver;

namespace Application.GroupChats.Commands.AddMemberToGroupChat
{
    public class AddMemberToGroupChatRequestHandler : RequestHandlerBase, IRequestHandler<AddMemberToGroupChatRequest>
    {
        public async Task Handle(AddMemberToGroupChatRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(request.ChatId);

            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            if (chat.Users.Any(u => u == request.NewMemberId))
                throw new NoPermissionsException("User is already a member of the chat");


            chat.Users.Add(request.NewMemberId);

            await Context.GroupChats.UpdateAsync(chat);
        }

        public AddMemberToGroupChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
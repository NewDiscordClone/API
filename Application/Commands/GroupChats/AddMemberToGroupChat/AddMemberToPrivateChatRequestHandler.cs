using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.GroupChats.AddMemberToGroupChat
{
    public class AddMemberToGroupChatRequestHandler : RequestHandlerBase, IRequestHandler<AddMemberToGroupChatRequest>
    {
        public async Task Handle(AddMemberToGroupChatRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(request.ChatId);

            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            if (chat.Users.Any(u => u.Id == request.NewMemberId))
                throw new NoPermissionsException("User is already a member of the chat");

            User newMember = await Context.SqlUsers.FindAsync(request.NewMemberId);

            chat.Users.Add(Mapper.Map<UserLookUp>(newMember));

            await Context.GroupChats.UpdateAsync(chat);
        }

        public AddMemberToGroupChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
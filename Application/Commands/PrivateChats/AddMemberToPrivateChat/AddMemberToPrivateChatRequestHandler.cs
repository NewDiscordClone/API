using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.AddMemberToPrivateChat
{
    public class AddMemberToPrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<AddMemberToPrivateChatRequest, PrivateChat>
    {
        public async Task<PrivateChat> Handle(AddMemberToPrivateChatRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            
            PrivateChat chat = await Context.PrivateChats.FindAsync(request.ChatId);
            
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            if (chat.Users.Any(u => u.Id == request.NewMemberId))
                throw new NoPermissionsException("User is already a member of the chat");

            User NewMember = await Context.FindSqlByIdAsync<User>(request.NewMemberId, cancellationToken);
            
            chat.Users.Add(Mapper.Map<UserLookUp>(NewMember));
            
            return await Context.PrivateChats.UpdateAsync(chat);
        }

        public AddMemberToPrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
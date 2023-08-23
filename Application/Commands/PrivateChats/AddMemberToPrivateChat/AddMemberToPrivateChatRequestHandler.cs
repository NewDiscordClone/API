using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.AddMemberToPrivateChat
{
    public class AddMemberToPrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<AddMemberToPrivateChatRequest>
    {
        public async Task Handle(AddMemberToPrivateChatRequest request, CancellationToken cancellationToken)
        {
            PrivateChat chat =
                await Context.FindByIdAsync<PrivateChat>(request.ChatId, cancellationToken);
            
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            if (chat.Users.Any(u => u.Id == request.NewMemberId))
                throw new NoPermissionsException("User is already a member of the chat");

            User NewMember = await Context.FindSqlByIdAsync<User>(request.NewMemberId, cancellationToken);
            
            await Context.Chats.UpdateOneAsync(
                Context.GetIdFilter<Chat>(chat.Id),
                Builders<Chat>.Update.Push(c => c.Users, Mapper.Map<UserLookUp>(NewMember)),
                null,
                cancellationToken
            );
        }

        public AddMemberToPrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
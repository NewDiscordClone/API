using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.PrivateChats.CreatePrivateChat
{
    public class CreatePrivateChatRequestHandler : RequestHandlerBase,  IRequestHandler<CreatePrivateChatRequest, Models.PrivateChat>
    {
        public async Task<PrivateChat> Handle(CreatePrivateChatRequest request, CancellationToken cancellationToken)
        {
            List<UserLookUp> users = new();
            request.UsersId.ForEach(userId => users.Add(Mapper.Map<UserLookUp>(Context.FindSqlByIdAsync<User>(userId, cancellationToken).Result)));
            
            User owner = await Context.FindSqlByIdAsync<User>(UserId, cancellationToken);
            PrivateChat privateChat = new()
            {
                Title = request.Title,
                //Image = request.Image,
                Users = users,
                OwnerId = owner.Id
            };

            await Context.PrivateChats.InsertOneAsync(privateChat, null, cancellationToken);
            return privateChat;
        }

        public CreatePrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
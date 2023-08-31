using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.PrivateChats.CreatePrivateChat
{
    public class CreatePrivateChatRequestHandler : RequestHandlerBase, IRequestHandler<CreatePrivateChatRequest,string>
    {
        public async Task<string> Handle(CreatePrivateChatRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            List<UserLookUp> users = new();
            request.UsersId.ForEach(userId => users.Add(Mapper.Map<UserLookUp>(Context.FindSqlByIdAsync<User>(userId, cancellationToken).Result)));

            PrivateChat privateChat = new GroupChat()
            {
                Title = request.Title,
                Image = request.Image,
                Users = users,
                OwnerId = UserId
            };

            return (await Context.PrivateChats.AddAsync(privateChat)).Id;
        }

        public CreatePrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
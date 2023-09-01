using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.GroupChats.CreateGroupChat
{
    public class CreateGroupChatRequestHandler : RequestHandlerBase, IRequestHandler<CreateGroupChatRequest,string>
    {
        public async Task<string> Handle(CreateGroupChatRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            List<UserLookUp> users = new();
            request.UsersId.ForEach(userId => users.Add(Mapper.Map<UserLookUp>(Context.FindSqlByIdAsync<User>(userId, cancellationToken).Result)));

            GroupChat privateChat = new GroupChat()
            {
                Title = request.Title,
                Image = request.Image,
                Users = users,
                OwnerId = UserId
            };

            return (await Context.GroupChats.AddAsync(privateChat)).Id;
        }

        public CreateGroupChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
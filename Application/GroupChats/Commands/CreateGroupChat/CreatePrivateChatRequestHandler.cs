using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.CreateGroupChat
{
    public class CreateGroupChatRequestHandler : RequestHandlerBase, IRequestHandler<CreateGroupChatRequest, string>
    {
        public async Task<string> Handle(CreateGroupChatRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            List<Guid> users = new();
            request.UsersId.ForEach(userId => users.Add(userId));

            GroupChat privateChat = new()
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
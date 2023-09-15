using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.CreateGroupChat
{
    public class CreateGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<CreateGroupChatCommand, string>
    {
        public async Task<string> Handle(CreateGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            List<Guid> users = new();
            command.UsersId.ForEach(users.Add);
            users.Add(UserId);

            GroupChat privateChat = new()
            {
                Title = command.Title,
                Image = command.Image,
                Users = users,
                OwnerId = UserId
            };

            return (await Context.GroupChats.AddAsync(privateChat)).Id;
        }

        public CreateGroupChatCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
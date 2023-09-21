using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Queries.GroupChatDetails
{
    public class GroupChatDetailsQueryHandler : RequestHandlerBase, IRequestHandler<GroupChatDetailsQuery, GroupChat>
    {
        public async Task<GroupChat> Handle(GroupChatDetailsQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(query.ChatId);
            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            return Mapper.Map<GroupChat>(chat);
        }

        public GroupChatDetailsQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
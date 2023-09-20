namespace Sparkle.Application.GroupChats.Queries.GroupChatDetails
{
    public class GroupChatDetailsQueryHandler : RequestHandlerBase, IRequestHandler<GroupChatDetailsQuery, GroupChat>
    {
        public async Task<GroupChat> Handle(GroupChatDetailsQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(query.ChatId);

            if (!chat.Profiles.Any(p => p.UserId == UserId))
                throw new NoPermissionsException("User is not a member of the chat");

            return chat;
        }

        public GroupChatDetailsQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
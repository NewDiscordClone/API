using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.GroupChats.Queries.GetGroupChatDetails
{
    public class GetGroupChatDetailsRequestHandler : RequestHandlerBase, IRequestHandler<GetGroupChatDetailsRequest, GroupChat>
    {
        public async Task<GroupChat> Handle(GetGroupChatDetailsRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            return Mapper.Map<GroupChat>(chat);
        }

        public GetGroupChatDetailsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
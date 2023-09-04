using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetGroupChatDetails
{
    public class GetGroupChatDetailsRequestHandler : RequestHandlerBase, IRequestHandler<GetGroupChatDetailsRequest, GroupChat>
    {
        public async Task<GroupChat> Handle(GetGroupChatDetailsRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            return Mapper.Map<GroupChat>(chat);
        }

        public GetGroupChatDetailsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
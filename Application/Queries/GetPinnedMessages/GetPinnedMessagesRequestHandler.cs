using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetPinnedMessages
{
    public class GetPinnedMessagesRequestHandler : RequestHandlerBase,
        IRequestHandler<GetPinnedMessagesRequest, List<GetPinnedMessageLookUpDto>>
    {
        public async Task<List<GetPinnedMessageLookUpDto>> Handle(GetPinnedMessagesRequest request,
            CancellationToken cancellationToken)
        {
            Chat chat = await Context.FindByIdAsync<Chat>(request.ChatId, cancellationToken, "Users", "Messages");
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);
            
            if (!chat.Users.Contains(user)) throw new NoPermissionsException("You are not a member of the Chat");
            
            return Mapper.Map<List<GetPinnedMessageLookUpDto>>(chat.PinnedMessages);
        }

        public GetPinnedMessagesRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(
            context, userProvider, mapper)
        {
        }
    }
}
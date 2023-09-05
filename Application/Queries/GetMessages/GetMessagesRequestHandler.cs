using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Models.LookUps;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetMessages
{
    public class GetMessagesRequestHandler : RequestHandlerBase, IRequestHandler<GetMessagesRequest, List<MessageDto>>
    {
        private const int _pageSize = 25;

        public async Task<List<MessageDto>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Chat chat = await Context.Chats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u == UserId)) throw new NoPermissionsException("You are not a member of the Chat");

            List<MessageDto> result = new List<MessageDto>();
            foreach (var message in await Context.GetMessagesAsync(request.ChatId, request.MessagesCount, _pageSize))
            {
                var res = Mapper.Map<MessageDto>(message);
                res.User = Mapper.Map<UserLookUp>(await Context.SqlUsers.FindAsync(message.User));
                result.Add(res);
            }
            return result;
        }

        public GetMessagesRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}
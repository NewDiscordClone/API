using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Messages.Queries.GetMessages
{
    public class GetMessagesRequestHandler : RequestHandlerBase, IRequestHandler<GetMessagesRequest, List<MessageDto>>
    {
        private const int _pageSize = 25;

        public async Task<List<MessageDto>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Chat chat = await Context.Chats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            List<MessageDto> result = new();
            foreach (Message message in await Context.GetMessagesAsync(request.ChatId, request.MessagesCount, _pageSize))
            {
                MessageDto res = Mapper.Map<MessageDto>(message);
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
using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Messages.Queries.GetMessages
{
    public class GetMessagesQueryHandler : RequestHandlerBase, IRequestHandler<GetMessagesQuery, List<MessageDto>>
    {
        private const int _pageSize = 25;

        public async Task<List<MessageDto>> Handle(GetMessagesQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Chat chat = await Context.Chats.FindAsync(query.ChatId);
            if (!chat.Profiles.Any(p => p.UserId == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            List<MessageDto> result = new();
            foreach (Message message in await Context.GetMessagesAsync(query.ChatId, query.MessagesCount, _pageSize))
            {
                MessageDto res = Mapper.Map<MessageDto>(message);
                res.User = Mapper.Map<UserLookUp>(await Context.SqlUsers.FindAsync(message.User));
                result.Add(res);
            }
            return result;
        }

        public GetMessagesQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}
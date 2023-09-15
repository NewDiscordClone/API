using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Messages.Queries.GetPinnedMessages
{
    public class GetPinnedMessagesQueryHandler : RequestHandlerBase,
        IRequestHandler<GetPinnedMessagesQuery, List<MessageDto>>
    {
        public async Task<List<MessageDto>> Handle(GetPinnedMessagesQuery query,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Chat chat = await Context.Chats.FindAsync(query.ChatId);

            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            List<Message> messages = await Context.GetPinnedMessagesAsync(chat.Id);
            return messages.ConvertAll(Mapper.Map<MessageDto>);
        }

        public GetPinnedMessagesQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(
            context, userProvider, mapper)
        {
        }
    }
}
using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Queries.GetPinnedMessages
{
    public class GetPinnedMessagesRequestHandler : RequestHandlerBase,
        IRequestHandler<GetPinnedMessagesRequest, List<Message>>
    {
        public async Task<List<Message>> Handle(GetPinnedMessagesRequest request,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Chat chat = await Context.Chats.FindAsync(request.ChatId);

            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            return await Context.GetPinnedMessagesAsync(chat.Id);
        }

        public GetPinnedMessagesRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(
            context, userProvider, mapper)
        {
        }
    }
}
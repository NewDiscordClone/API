using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetPinnedMessages
{
    public class GetPinnedMessagesRequestHandler : RequestHandlerBase,
        IRequestHandler<GetPinnedMessagesRequest, List<Message>>
    {
        public async Task<List<Message>> Handle(GetPinnedMessagesRequest request,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Chat chat = await Context.Chats.FindAsync(request.ChatId);

            if (!chat.Users.Any(u => u.Id == UserId))
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
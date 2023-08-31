using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetMessages
{
    public class GetMessagesRequestHandler : RequestHandlerBase, IRequestHandler<GetMessagesRequest, List<Message>>
    {
        private const int _pageSize = 25;

        public async Task<List<Message>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Chat chat = await Context.Chats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u.Id == UserId)) throw new NoPermissionsException("You are not a member of the Chat");

            return await Context.GetMessagesAsync(request.ChatId, request.MessagesCount, _pageSize);
        }

        public GetMessagesRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}
using Application.Exceptions;
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
            Chat chat = await Context.FindByIdAsync<Chat>(request.ChatId, cancellationToken);
            
            if (chat.Users.Find(u => u.Id == UserId) == null) throw new NoPermissionsException("You are not a member of the Chat");

            return await chat.GetPinnedMessagesAsync(Context.Messages, cancellationToken);
        }

        public GetPinnedMessagesRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(
            context, userProvider, mapper)
        {
        }
    }
}
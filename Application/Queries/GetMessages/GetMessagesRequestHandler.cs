using Application.Interfaces;
using Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Queries.GetMessages
{
    public class GetMessagesRequestHandler : RequestHandlerBase, IRequestHandler<GetMessagesRequest, List<Message>>
    {
        private const int _pageSize = 25;

        public async Task<List<Message>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            
            var messages = await
            (
                await Context.Chats.FindAsync(request.ChatId)
            ).GetMessagesAsync
            (
                Context.Messages.Collection, request.MessagesCount, _pageSize, cancellationToken
            );
            return messages;
        }

        public GetMessagesRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
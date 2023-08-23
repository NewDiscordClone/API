using Application.Interfaces;
using Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Queries.GetMessages
{
    public class GetMessagesRequestHandler : IRequestHandler<GetMessagesRequest, List<Message>>
    {
        private const int _pageSize = 25;
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetMessagesRequestHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<Message>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
        {
            var messages = await
            (
                await _appDbContext.FindByIdAsync<Chat>(request.ChatId, cancellationToken)
            ).GetMessagesAsync
            (
                _appDbContext.Messages, request.MessagesCount, _pageSize, cancellationToken
            );
            return messages;
        }
    }
}
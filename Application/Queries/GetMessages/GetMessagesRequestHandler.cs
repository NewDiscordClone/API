using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GetMessages
{
    public class GetMessagesRequestHandler : IRequestHandler<GetMessagesRequest, List<GetMessageLookUpDto>>
    {
        private const int _pageSize = 25;
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetMessagesRequestHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<GetMessageLookUpDto>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
        {

            List<GetMessageLookUpDto> messages = await _appDbContext.Messages
                .Where(m => m.Chat.Id == request.ChatId)
                .OrderBy(m => m.SendTime)
                .Skip(request.MessagesCount)
                .Take(_pageSize)
                .ProjectTo<GetMessageLookUpDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return messages;
        }
    }
}
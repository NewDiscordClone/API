using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RequestModels.GetMessages
{
    public class GetMessagesRequestHandler: IRequestHandler<GetMessagesRequest, List<GetMessageDto>>
    {
        private const int _pageSize = 25;
        private readonly IAppDbContext _appDbContext;

        public GetMessagesRequestHandler(IAppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        
        public async Task<List<GetMessageDto>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
        {
            var chat = await _appDbContext.Chats.FindAsync(new object[] { request.ChatId }, cancellationToken: cancellationToken);
            List<GetMessageDto> messages = new();
            await _appDbContext.Messages
                .Where(m => m.Chat.Id == chat.Id)
                .OrderBy(m => m.SendTime)
                .Skip(request.MessagesCount)
                .Take(_pageSize)
                .ForEachAsync(m => messages.Add(Convertors.Convert(m)), cancellationToken: cancellationToken);
            return messages;
        }
    }
}
using Application.Interfaces;
using Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RequestModels.GetPrivateChats
{
    public class GetPrivateChatsRequestHandler :  IRequestHandler<GetPrivateChatsRequest, List<GetPrivateChatDto>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetPrivateChatsRequestHandler(IAppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<List<GetPrivateChatDto>> Handle(GetPrivateChatsRequest request, CancellationToken cancellationToken)
        {
            var user = await _appDbContext.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
            if (user == null) throw new NoSuchUserException();
            List<GetPrivateChatDto> privateChat = new();
            await _appDbContext.PrivateChats
                .Where(chat => chat.Users.Contains(user))
                .ForEachAsync(pc => privateChat.Add(Convertors.Convert(pc)), cancellationToken: cancellationToken);
            return privateChat;
        }
    }
}
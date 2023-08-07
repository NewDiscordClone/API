using Application.Interfaces;
using Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RequestModels.PrivateChats
{
    public class GetPrivateChatsRequestHandler :  IRequestHandler<GetPrivateChatsRequest, List<PrivateChat>>
    {
        private readonly IAppDbContext appDbContext;

        public GetPrivateChatsRequestHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<PrivateChat>> Handle(GetPrivateChatsRequest request, CancellationToken cancellationToken)
        {
            var user = await appDbContext.Users.FindAsync(new object[] { request.UserID }, cancellationToken: cancellationToken);
            return await appDbContext.PrivateChats.Where(chat
                    => chat.Users.Contains(user))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
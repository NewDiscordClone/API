using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Queries.GetPrivateChats
{
    public class GetPrivateChatsRequestHandler : RequestHandlerBase,
        IRequestHandler<GetPrivateChatsRequest, List<PrivateChat>>
    {
        public GetPrivateChatsRequestHandler(IAppDbContext appDbContext, IAuthorizedUserProvider userProvider,
            IMapper mapper)
            : base(appDbContext, userProvider, mapper)
        {
        }

        public async Task<List<PrivateChat>> Handle(GetPrivateChatsRequest request,
            CancellationToken cancellationToken)
        {
            List<PrivateChat> privateChat = await (await Context.PrivateChats
                    .FindAsync(Builders<PrivateChat>.Filter
                            .ElemMatch(c => c.Users, u => u.Id == UserId),
                        null,
                        cancellationToken))
                .ToListAsync(cancellationToken);
            return privateChat;
        }
    }
}
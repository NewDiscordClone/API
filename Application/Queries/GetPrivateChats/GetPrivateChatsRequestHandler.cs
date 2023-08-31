using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;
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
            Context.SetToken(cancellationToken);

            return await Context.PrivateChats.FilterAsync(c => c.Users.Any(u => u.Id == UserId));
        }
    }
}
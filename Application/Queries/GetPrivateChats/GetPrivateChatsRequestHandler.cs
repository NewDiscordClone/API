using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GetPrivateChats
{
    public class GetPrivateChatsRequestHandler : IRequestHandler<GetPrivateChatsRequest, List<GetPrivateChatLookUpDto>>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetPrivateChatsRequestHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<GetPrivateChatLookUpDto>> Handle(GetPrivateChatsRequest request, CancellationToken cancellationToken)
        {
            User user = await _appDbContext.Users.FindAsync(new object[] { request.UserId }, cancellationToken)
                ?? throw new NoSuchUserException();

            List<GetPrivateChatLookUpDto> privateChat = await _appDbContext.PrivateChats
                .Where(chat => chat.Users.Contains(user))
                .ProjectTo<GetPrivateChatLookUpDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return privateChat;
        }
    }
}
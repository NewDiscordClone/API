using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetPrivateChatDetails
{
    public class GetPrivateChatDetailsRequestHandler : IRequestHandler<GetPrivateChatDetailsRequest, GetPrivateChatDetailsDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetPrivateChatDetailsRequestHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetPrivateChatDetailsDto> Handle(GetPrivateChatDetailsRequest request, CancellationToken cancellationToken)
        {
            User user = await _context.FindByIdAsync<User>(request.UserId, cancellationToken);
            PrivateChat chat = await _context.FindByIdAsync<PrivateChat>(request.ChatId, cancellationToken);
            if (chat.Users.Find(u => u.Id == user.Id) == null)
                throw new NoPermissionsException("User is not a member of the chat");
            return _mapper.Map<GetPrivateChatDetailsDto>(chat);

        }
    }
}
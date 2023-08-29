using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetPrivateChatDetails
{
    public class GetPrivateChatDetailsRequestHandler : RequestHandlerBase, IRequestHandler<GetPrivateChatDetailsRequest, PrivateChat>
    {
        public async Task<PrivateChat> Handle(GetPrivateChatDetailsRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            
            PrivateChat chat = await Context.PrivateChats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            return Mapper.Map<PrivateChat>(chat);
        }

        public GetPrivateChatDetailsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
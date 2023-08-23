using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetPrivateChatDetails
{
    public class GetPrivateChatDetailsRequestHandler : RequestHandlerBase, IRequestHandler<GetPrivateChatDetailsRequest, PrivateChat>
    {
        public async Task<PrivateChat> Handle(GetPrivateChatDetailsRequest request, CancellationToken cancellationToken)
        {
            PrivateChat chat = await Context.FindByIdAsync<PrivateChat>(request.ChatId, cancellationToken);
            if (chat.Users.Find(u => u.Id == UserId) == null)
                throw new NoPermissionsException("User is not a member of the chat");
            return Mapper.Map<PrivateChat>(chat);
        }

        public GetPrivateChatDetailsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
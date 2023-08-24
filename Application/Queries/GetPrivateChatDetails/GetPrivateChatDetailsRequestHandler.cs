using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetPrivateChatDetails
{
    public class GetPrivateChatDetailsRequestHandler : RequestHandlerBase, IRequestHandler<GetPrivateChatDetailsRequest, GetPrivateChatDetailsDto>
    {
        public async Task<GetPrivateChatDetailsDto> Handle(GetPrivateChatDetailsRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);
            PrivateChat chat = await Context.FindByIdAsync<PrivateChat>(request.ChatId, cancellationToken, "Users");
            if (chat.Users.Find(u => u.Id == user.Id) == null)
                throw new NoPermissionsException("User is not a member of the chat");
            return Mapper.Map<GetPrivateChatDetailsDto>(chat);
        }

        public GetPrivateChatDetailsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
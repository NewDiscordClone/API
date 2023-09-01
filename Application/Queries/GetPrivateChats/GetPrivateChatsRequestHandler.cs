using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;
using MongoDB.Driver;

namespace Application.Queries.GetPersonalChats
{
    public class GetPrivateChatsRequestHandler : RequestHandlerBase,
        IRequestHandler<GetPrivateChatsRequest, List<GetPrivateChatLookUpDto>>
    {
        public GetPrivateChatsRequestHandler(IAppDbContext appDbContext, IAuthorizedUserProvider userProvider,
            IMapper mapper)
            : base(appDbContext, userProvider, mapper)
        {
        }

        public async Task<List<GetPrivateChatLookUpDto>> Handle(GetPrivateChatsRequest request,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            List<GetPrivateChatLookUpDto> chats = new();
            foreach (var personalChat in await Context.PersonalChats.FilterAsync(c => c.Users.Any(u => u.Id == UserId)))
            {
                if(personalChat is GroupChat gchat) chats.Add(Mapper.Map<GetPrivateChatLookUpDto>(gchat));
                else
                {
                    UserLookUp other = personalChat.Users.First(u => u.Id != UserId);
                    chats.Add(new GetPrivateChatLookUpDto()
                    {
                        Id = personalChat.Id,
                        Users = personalChat.Users,
                        Image = other.Avatar,
                        Title = other.DisplayName,
                        Subtitle = other.TextStatus
                    });
                }
                
            }
            return chats;
        }
    }
}
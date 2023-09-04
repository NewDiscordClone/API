using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;
using MongoDB.Driver;

namespace Application.Queries.GetPersonalChats
{
    public class GetPersonalChatsHandler : RequestHandlerBase,
        IRequestHandler<GetPersonalChatsRequest, List<PrivateChatLookUp>>
    {
        public GetPersonalChatsHandler(IAppDbContext appDbContext, IAuthorizedUserProvider userProvider,
            IMapper mapper)
            : base(appDbContext, userProvider, mapper)
        {
        }

        public async Task<List<PrivateChatLookUp>> Handle(GetPersonalChatsRequest request,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            List<PrivateChatLookUp> chats = new();
            foreach (PersonalChat personalChat in await Context.PersonalChats.FilterAsync(c => c.Users.Any(u => u.Id == UserId)))
            {
                if (personalChat is GroupChat gchat)
                    chats.Add(Mapper.Map<PrivateChatLookUp>(gchat));
                else
                {
                    chats.Add(new PrivateChatLookUp(personalChat, UserId));
                }

            }
            return chats;
        }
    }
}
using Application.Interfaces;
using Application.Models;
using Application.Models.LookUps;
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
            foreach (var personalChat in await Context.PersonalChats.FilterAsync(c => c.Users.Any(u => u == UserId)))
            {
                if (personalChat is GroupChat gchat) 
                    chats.Add(Mapper.Map<PrivateChatLookUp>(gchat));
                else
                {
                    if (!personalChat.Users.Any(u => u != UserId))
                        throw new AggregateException("There is no other user");
                    Guid userid = personalChat.Users.First(u => u != UserId);
                    chats.Add(
                        new PrivateChatLookUp(
                            personalChat,
                            Mapper.Map<UserLookUp>(
                                await Context.SqlUsers.FindAsync(userid)
                            )
                        )
                    );
                }
            }

            return chats;
        }
    }
}
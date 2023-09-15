using AutoMapper;
using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.GroupChats.Queries.GetPrivateChats
{
    public class GetPrivateChatsQueryHandler : RequestHandlerBase,
        IRequestHandler<GetPrivateChatsQuery, List<PrivateChatLookUp>>
    {
        public GetPrivateChatsQueryHandler(IAppDbContext appDbContext, IAuthorizedUserProvider userProvider,
            IMapper mapper)
            : base(appDbContext, userProvider, mapper)
        {
        }

        public async Task<List<PrivateChatLookUp>> Handle(GetPrivateChatsQuery query,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            List<PrivateChatLookUp> chats = new();
            foreach (PersonalChat personalChat in await Context.PersonalChats.FilterAsync(c => c.Users.Any(u => u == UserId)))
            {
                if (personalChat is GroupChat gchat)
                {
                    PrivateChatLookUp lookUp = Mapper.Map<PrivateChatLookUp>(gchat);
                    if (string.IsNullOrWhiteSpace(gchat.Title))
                    {
                        lookUp.Title = string.Join
                        (", ",
                            (await Context.SqlUsers
                                .FilterAsync(u => gchat.Users.Contains(u.Id) && u.Id != UserId))
                            .Select(u => u.DisplayName ?? u.UserName)
                        );
                    }
                    chats.Add(lookUp);
                }
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
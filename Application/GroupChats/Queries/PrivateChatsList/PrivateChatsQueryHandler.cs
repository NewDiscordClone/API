using AutoMapper;
using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.GroupChats.Queries.PrivateChatsList
{
    public class PrivateChatsQueryHandler : RequestHandlerBase,
        IRequestHandler<PrivateChatsQuery, List<PrivateChatLookUp>>
    {
        public PrivateChatsQueryHandler(IAppDbContext appDbContext, IAuthorizedUserProvider userProvider,
            IMapper mapper)
            : base(appDbContext, userProvider, mapper)
        {
        }

        public async Task<List<PrivateChatLookUp>> Handle(PrivateChatsQuery query,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            List<PrivateChatLookUp> chats = new();

            foreach (PersonalChat personalChat in await Context.PersonalChats.FilterAsync(c => c.Profiles.Any(p => p.UserId == UserId)))
            {
                if (personalChat is GroupChat gchat)
                {
                    PrivateChatLookUp lookUp = Mapper.Map<PrivateChatLookUp>(gchat);
                    if (string.IsNullOrWhiteSpace(gchat.Title))
                    {
                        lookUp.Title = string.Join
                        (", ",
                            (await Context.SqlUsers
                                .FilterAsync(u => u.Id != UserId))
                            .Where(u => gchat.Profiles.Any(p => p.UserId == u.Id))
                            .Select(u => u.DisplayName ?? u.UserName)
                            .AsEnumerable()
                        );
                    }

                    chats.Add(lookUp);
                }
                else
                {
                    if (!personalChat.Profiles.Any(u => u.UserId != UserId))
                        throw new AggregateException("There is no other user");
                    Guid userid = personalChat.Profiles.Select(p => p.UserId).First(id => id != UserId);
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
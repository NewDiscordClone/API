using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatCreated
{
    public class NotifyPrivateChatCreatedQueryHandler : HubRequestHandlerBase, IRequestHandler<NotifyPrivateChatCreatedQuery>
    {
        public NotifyPrivateChatCreatedQueryHandler(
            IHubContextProvider hubContextProvider,
            IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(hubContextProvider, context, userProvider, mapper)
        {
        }

        public async Task Handle(NotifyPrivateChatCreatedQuery query, CancellationToken cancellationToken)
        {
            SetToken(cancellationToken);
            Chat chat = await Context.PersonalChats.FindAsync(query.ChatId);
            User user = await Context.SqlUsers.FindAsync(UserId);

            PrivateChatLookUp lookUp = chat switch
            {
                GroupChat gChat => Mapper.Map<PrivateChatLookUp>(gChat),
                PersonalChat pChat => new PrivateChatLookUp(pChat, Mapper.Map<UserLookUp>(user)),
                _ => throw new ArgumentException("The given chat is not an private chat")
            };

            await SendAsync(ClientMethods.PrivateChatCreated, lookUp, GetConnections(chat));
        }
    }
}
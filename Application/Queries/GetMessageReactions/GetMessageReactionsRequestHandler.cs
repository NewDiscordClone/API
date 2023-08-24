using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetMessageReactions
{
    public class GetMessageReactionsRequestHandler : RequestHandlerBase,
        IRequestHandler<GetMessageReactionsRequest, List<UserReactionDto>>
    {
        public async Task<List<UserReactionDto>> Handle(GetMessageReactionsRequest request,
            CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken,
                "User",
                "Chat",
                "Chat.Users",
                "Reactions",
                "Reactions.User");

            if (!message.Chat.Users.Contains(user))
                throw new NoPermissionsException("You are not a member of the Chat");


            List<UserReactionDto> userReactions = new();
            message.Reactions.ForEach(r =>
                userReactions.Add(new UserReactionDto
                {
                    User = Mapper.Map<GetUserLookUpDto>(r.User),
                    Reaction = Mapper.Map<GetReactionDto>(r)
                })
            );

            return userReactions;
        }

        public GetMessageReactionsRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(context, userProvider, mapper)
        {
        }
    }
}
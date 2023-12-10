using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;
using Sparkle.Domain.LookUps;
using Sparkle.Domain.Messages;

namespace Sparkle.Application.Messages.Queries.GetMessages
{
    public class GetMessagesQueryHandler : RequestHandlerBase, IRequestHandler<GetMessagesQuery, List<MessageDto>>
    {
        private const int _pageSize = 50;

        public async Task<List<MessageDto>> Handle(GetMessagesQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            List<MessageDto> result = new();
            foreach (Message message in await Context.GetMessagesAsync(query.ChatId, query.MessagesCount, _pageSize))
            {
                MessageDto res = Mapper.Map<MessageDto>(message);

                UserProfile? userProfile = await Context.UserProfiles
                    .FindAsync(new object?[] { message.AuthorProfile }, cancellationToken: cancellationToken)
                    ?? throw new EntityNotFoundException(message.AuthorProfile);

                User? user = await Context.Users.FindAsync(new object?[] { userProfile.UserId }, cancellationToken: cancellationToken) ??
                    throw new EntityNotFoundException(userProfile.UserId);

                res.Author = Mapper.Map<UserViewModel>(user);

                if (userProfile is not null and ServerProfile serverProfile)
                {
                    res.Author.DisplayName = serverProfile.DisplayName
                        ?? res.Author.DisplayName;
                }

                result.Add(res);
            }
            return result;
        }

        public GetMessagesQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}
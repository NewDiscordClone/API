using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Chats.PersonalChats.Queries
{
    public class GetPersonalChatByUserIdQueryHandler : RequestHandlerBase,
        IRequestHandler<GetPersonalChatByUserIdQuery, PrivateChatViewModel>
    {
        private readonly IConvertor _convertor;
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IChatRepository _chatRepository;

        public GetPersonalChatByUserIdQueryHandler(IAuthorizedUserProvider userProvider,
            IConvertor convertor,
            IRelationshipRepository relationshipRepository,
            IChatRepository chatRepository)
            : base(userProvider)
        {
            _convertor = convertor;
            _relationshipRepository = relationshipRepository;
            _chatRepository = chatRepository;
        }

        public async Task<PrivateChatViewModel> Handle(GetPersonalChatByUserIdQuery query, CancellationToken cancellationToken)
        {
            Relationship relationship = await _relationshipRepository.FindAsync((query.UserId, UserId), cancellationToken);

            if (relationship.PersonalChatId is null)
                throw new EntityNotFoundException($"Chat with user {query.UserId} does not exists", query.UserId);

            PersonalChat chat = await _chatRepository.FindAsync<PersonalChat>(relationship.PersonalChatId, cancellationToken);

            return await _convertor.ConvertToViewModelAsync(chat, cancellationToken);
        }
    }
}

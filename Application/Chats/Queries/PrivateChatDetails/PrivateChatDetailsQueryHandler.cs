using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Chats.Queries.PrivateChatDetails
{
    public class PrivateChatDetailsQueryHandler : RequestHandler, IRequestHandler<PrivateChatDetailsQuery, PrivateChatViewModel>
    {
        private readonly IConvertor _convertor;
        private readonly IChatRepository _chatRepository;

        public PrivateChatDetailsQueryHandler(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IConvertor convertor,
            IChatRepository chatRepository)
            : base(userProvider, mapper)
        {
            _convertor = convertor;
            _chatRepository = chatRepository;
        }

        public async Task<PrivateChatViewModel> Handle(PrivateChatDetailsQuery query, CancellationToken cancellationToken)
        {
            PersonalChat chat = await _chatRepository.FindAsync<PersonalChat>(query.ChatId, cancellationToken);

            return await _convertor.ConvertToViewModelAsync(chat, cancellationToken);
        }

    }
}
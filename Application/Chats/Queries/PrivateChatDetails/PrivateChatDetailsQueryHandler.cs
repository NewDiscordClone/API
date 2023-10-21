using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Chats.Queries.PrivateChatDetails
{
    public class PrivateChatDetailsQueryHandler : RequestHandlerBase, IRequestHandler<PrivateChatDetailsQuery, PrivateChatViewModel>
    {
        private readonly IConvertor _convertor;
        public PrivateChatDetailsQueryHandler(IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IConvertor convertor)
            : base(context, userProvider, mapper)
        {
            _convertor = convertor;
        }

        public async Task<PrivateChatViewModel> Handle(PrivateChatDetailsQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            PersonalChat chat = await Context.PersonalChats.FindAsync(query.ChatId, cancellationToken);

            return await _convertor.ConvertToViewModelAsync(chat, cancellationToken);
        }

    }
}
using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Messages.Commands.UnpinMessage
{
    public class UnpinMessageCommandHandler : RequestHandlerBase, IRequestHandler<UnpinMessageCommand, MessageDto>
    {
        public async Task<MessageDto> Handle(UnpinMessageCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(request.MessageId);
            Chat chat = await Context.Chats.FindAsync(message.ChatId);

            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            //TODO: Перевірка на відповідну роль на сервері

            message.IsPinned = false;

            return Mapper.Map<MessageDto>(await Context.Messages.UpdateAsync(message));
        }

        public UnpinMessageCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
            : base(context, userProvider, mapper)
        {
        }
    }
}
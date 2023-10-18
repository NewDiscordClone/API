using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveMessage
{
    public class RemoveMessageCommandHandler : RequestHandlerBase, IRequestHandler<RemoveMessageCommand, Chat>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly IServerProfileRepository _serverProfileRepository;
        public async Task<Chat> Handle(RemoveMessageCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId, cancellationToken);

            if (message.ChatId != command.ChatId)
                throw new InvalidOperationException("The specified message does not belong to the given chat.");

            Chat chat = await Context.Chats.FindAsync(message.ChatId, cancellationToken);

            UserProfile? profile = await _profileRepository
             .FindOrDefaultByChatIdAndUserIdAsync(chat.Id, UserId, cancellationToken);

            profile ??= await _serverProfileRepository
                .FindProfileByChannelIdAsync(chat.Id, UserId, cancellationToken);

            if (message.Author != UserId
                || UserProvider.HasClaims(profile!, Constants.Claims.RemoveMessages)
                || UserProvider.IsAdmin(profile!))
            {
                throw new NoPermissionsException("You don't have permission to remove the message");
            }


            await Context.Messages.DeleteAsync(message, cancellationToken);
            return chat;
        }

        public RemoveMessageCommandHandler(IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IUserProfileRepository profileRepository,
            IServerProfileRepository serverProfileRepository)
            : base(context,
            userProvider)
        {
            _profileRepository = profileRepository;
            _serverProfileRepository = serverProfileRepository;
        }
    }
}
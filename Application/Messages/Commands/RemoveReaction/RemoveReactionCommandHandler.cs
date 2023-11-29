using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveReaction
{
    public class RemoveReactionCommandHandler : RequestHandlerBase, IRequestHandler<RemoveReactionCommand>
    {
        private readonly Common.Interfaces.Repositories.IUserProfileRepository _userProfileRepository;

        public async Task Handle(RemoveReactionCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId, cancellationToken);
            Channel? channel = await Context.Channels.FindOrDefaultAsync(message.ChatId, cancellationToken);
            UserProfile profile;
            if (channel == null)
                profile = await Context.UserProfiles
                    .FirstAsync(p => p.UserId == UserId && p.ChatId == message.ChatId, cancellationToken);
            else
                profile = await Context.UserProfiles.OfType<ServerProfile>()
                    .FirstAsync(p => p.UserId == UserId && p.ServerId == channel.ServerId, cancellationToken);
            
            Reaction? reaction = message.Reactions.Find(e => e.Emoji == command.Emoji && e.AuthorProfile == profile.Id);
            if (reaction == null)
                throw new InvalidOperationException($"Reaction {command.Emoji} does not exists");

            message.Reactions.Remove(reaction);

            await Context.Messages.UpdateAsync(message, cancellationToken);
        }

        public RemoveReactionCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            Common.Interfaces.Repositories.IUserProfileRepository userProfileRepository) : base(context,
            userProvider)
        {
            _userProfileRepository = userProfileRepository;
        }
    }
}
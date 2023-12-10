using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;
using Sparkle.Domain.Messages;
using Sparkle.Domain.Messages.ValueObjects;

namespace Sparkle.Application.Messages.Commands.AddReaction
{
    public class AddReactionCommandHandler : RequestHandlerBase, IRequestHandler<AddReactionCommand>
    {
        public async Task Handle(AddReactionCommand command, CancellationToken cancellationToken)
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

            Reaction reaction = new()
            {
                AuthorProfile = profile.Id,
                Emoji = command.Emoji,
            };

            if (message.Reactions.Contains(reaction))
                return;

            message.Reactions.Add(reaction);

            await Context.Messages.UpdateAsync(message, cancellationToken);
        }

        public AddReactionCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}